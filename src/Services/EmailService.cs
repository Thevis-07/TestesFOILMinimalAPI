using Microsoft.Extensions.Options;
using PreMailer.Net;
using Scriban;
using Scriban.Runtime;
using System.Net;
using System.Net.Mail;
using System.Text;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Options;
using TestesFOILMinimalApi.Dtos.Email;
using static TestesFOILMinimalApi.Dtos.Email.EmailDto;

namespace TestesFOILMinimalApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _smtp;
        private readonly string _templatesPath;

        public EmailService(IOptions<SmtpOptions> smtpOptions)
        {
            _smtp = smtpOptions.Value;
            var envPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            if (!string.IsNullOrWhiteSpace(envPassword))
                _smtp.Password = envPassword;

            _templatesPath = Path.Combine(AppContext.BaseDirectory, "Files", "EmailTemplates");
        }


        public async Task SendRelatorioCategoriasAsync(
            IEnumerable<CategoriaResumoItem> itens,
            string to,
            EmailDto.AlunoEmail aluno,
            string? subjectPrefix = null,
            CancellationToken ct = default)
        {
            var lista = itens?.ToList() ?? [];
            if (lista.Count == 0)
                throw new ArgumentException("A lista de itens está vazia.", nameof(itens));

            if (aluno is null || string.IsNullOrWhiteSpace(aluno.nome))
                throw new ArgumentException("O objeto 'aluno' deve ser informado com 'nome'.", nameof(aluno));

            var model = new EmailDto.EmailAluno(aluno, lista);

            var html = await RenderTemplateAsync("resultado-estilos-parentais.sbnhtml", model, ct);

            var sb = new StringBuilder()
                .AppendLine($"Resultado — {aluno.nome}");
            foreach (var i in lista)
            {
                sb.AppendLine($"{i.CategoriaAbreviacao}: Pai {i.TotalPai} | Mãe {i.TotalMae}");
                // Descomentar para adicionar descricao
                //if (!string.IsNullOrWhiteSpace(i.CategoriaDescricao))
                //    sb.AppendLine($"  -> {i.CategoriaDescricao}");
            }
            var textAlt = sb.ToString();

            var subject = $"{(string.IsNullOrWhiteSpace(subjectPrefix) ? "Resultado Estilos Parentais" : subjectPrefix)} — {aluno.nome}";
            await SendHtmlAsync(to, subject, html, textAlt, ct);
        }


        private async Task<string> RenderTemplateAsync(string templateName, object model, CancellationToken ct)
        {
            var fullPath = Path.Combine(_templatesPath, templateName);
            Console.WriteLine($"[EmailService] Template: {fullPath}");
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Template não encontrado: {fullPath}");

            var source = await File.ReadAllTextAsync(fullPath, ct);

            var tpl = Template.Parse(source);

            // 1) Erros de PARSE já na leitura do arquivo
            if (tpl.HasErrors)
            {
                Console.WriteLine("[Scriban] Parse errors:");
                foreach (var m in tpl.Messages)
                    Console.WriteLine($"");
                throw new InvalidOperationException("Template possui erros de sintaxe.");
            }

            var ctx = new TemplateContext
            {
                EnableRelaxedMemberAccess = true,                  
                MemberRenamer = m => ToSnakeCase(m.Name),          
                LoopLimit = 10000
            };

            var scriptObj = new Scriban.Runtime.ScriptObject();
            scriptObj.Import(model, renamer: m => ToSnakeCase(m.Name));
            ctx.PushGlobal(scriptObj);

            try
            {
                var html = await tpl.RenderAsync(ctx);
                return PreMailer.Net.PreMailer.MoveCssInline(html).Html;
            }
            catch (Scriban.Syntax.ScriptRuntimeException ex)
            {
                Console.WriteLine("[Scriban] Runtime error: " + ex.Message);
                if (!ex.Span.IsEmpty)
                    Console.WriteLine($"At line {ex.Span.Start.Line}, col {ex.Span.Start.Column}");
                throw;
            }
        }


        private static string ToSnakeCase(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            var chars = new List<char>(name.Length + 8);
            for (int i = 0; i < name.Length; i++)
            {
                var c = name[i];
                if (char.IsUpper(c))
                {
                    if (i > 0 && (char.IsLower(name[i - 1]) || (i + 1 < name.Length && char.IsLower(name[i + 1]))))
                        chars.Add('_');
                    chars.Add(char.ToLowerInvariant(c));
                }
                else
                {
                    chars.Add(c);
                }
            }
            return new string(chars.ToArray());
        }

        private async Task SendHtmlAsync(string to, string subject, string html, string? textAlt, CancellationToken ct)
        {
            try
            {
                using var mail = new MailMessage();
                mail.From = new MailAddress(_smtp.FromEmail, _smtp.FromName);
                mail.To.Add(to);
                mail.Subject = subject;

                var plain = AlternateView.CreateAlternateViewFromString(
                    textAlt ?? "Veja este e-mail em HTML.", Encoding.UTF8, "text/plain");
                var htmlView = AlternateView.CreateAlternateViewFromString(
                    html, Encoding.UTF8, "text/html");

                mail.AlternateViews.Add(plain);
                mail.AlternateViews.Add(htmlView);

                using var client = new SmtpClient(_smtp.Host, _smtp.Port)
                {
                    EnableSsl = _smtp.UseStartTls,
                    Credentials = new NetworkCredential(_smtp.User, _smtp.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"[EmailService] Erro ao enviar e-mail: {ex.GetType().Name} - {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[EmailService] InnerException: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");

                throw new InvalidOperationException("Falha ao enviar e-mail. Detalhes: " + ex.Message, ex);
            }
        }
    }
}
