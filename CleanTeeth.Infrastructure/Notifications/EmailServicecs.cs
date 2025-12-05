using CleanTeeth.Application.Notifications;
using CleanTeethApplication.Notifications;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Infrastructure.Notifications
{
    public class EmailServicecs : INotifications
    {
        private readonly IConfiguration configuration;

        public EmailServicecs(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendAppointmentConfimration(AppointmentConfirmationDTO appointmentConfirmationDTO)
        {
            var subject = "Appointment Confirmation - Clean Teeth";

            var body = $"Dear {appointmentConfirmationDTO.Patient},\n" +
                       $"Your appointment on {appointmentConfirmationDTO.Date} is confirmed.\n" +
                       $"Dentist: {appointmentConfirmationDTO.Dentist}\n" +
                       $"Office: {appointmentConfirmationDTO.DentalOffice}";

            // IMPORTANT: use Patient_Email instead of Patient
            await SendEmail(appointmentConfirmationDTO.Patient_Email, subject, body);
        }


        private async Task SendEmail(string to, string subject, string body)
        {
            var from = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:EMAIL");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:PASSWORD");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATIONS:PORT");

            using var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var message = new MailMessage(from!, to, subject, body);
            await smtpClient.SendMailAsync(message);
        }

    }
}
