﻿using System.Data;
using BCryptNet = BCrypt.Net.BCrypt;
using CrossCutting;
using Domain.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace Application.UseCases.UserOperations.CreateUser
{
    public class CreateUserUsecase : ICreateUserUsecase
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;
        private readonly IProducerService _producer;
        private readonly IConsumerService _consumer;
        public CreateUserUsecase(IUserRepository repository, IConfiguration config, IProducerService producer, IConsumerService consumer)
        {
            this._repository = repository;
            this._config = config;
            this._producer = producer;
            this._consumer = consumer;
        }

        public async Task<ResponseModel> CreateUser(CreateUserRequestModel createUserRequestModel)
        {
            if (_repository.Any(x => x.Username == createUserRequestModel.Username || x.Email == createUserRequestModel.Email))
            {
                return ResponseFactory.Conflict();
            }

            try
            {
                var (status, message) = await _producer.SendMessage(createUserRequestModel, "create-user");

                var res = JsonSerializer.Deserialize(message, typeof(CreateUserRequestModel));

                return res.Created(message = status);
            }
            catch (DBConcurrencyException)
            {
                return ResponseFactory.NotAcceptable("Formato de dados inválido");
            }
            catch (InvalidOperationException)
            {
                return ResponseFactory.NotAcceptable("Formato de dados inválido.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public string SendEmailToVerify(CreateUserResponse user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException($"Valor de email não pode ser nulo. at {System.Environment.CurrentDirectory}");
            }
            string host = _config.GetSection("Host").Value;

            var verificationLink = $"{host}auth/verify/{user.UserId}";

            var template = File.ReadAllText(
                Path.Combine(
                    _config["BasePath"],
                    "Static",
                    "verify-template.html"
                )
            );

            var body = template
                .Replace("#verificationLink", verificationLink)
                .Replace("#userName", user.Username);

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(body);
            System.Console.ForegroundColor = ConsoleColor.White;

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "RetroCollect Password Recover";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);

            return "Email sent";
            
        }
    }
}
