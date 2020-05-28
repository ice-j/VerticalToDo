using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Abstractions;
using VerticalToDo.Abstractions.Exceptions;
using VerticalToDo.Services;

namespace VerticalToDo.Infrastructure.Decorators
{
    public class MediatorPipelineHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
                where TRequest : IRequest<TResponse>
                where TResponse : class
    {
        private readonly IRequestHandler<TRequest, TResponse> _handler;
        private readonly IConfigurationService _configurationService;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public MediatorPipelineHandler(
                                IRequestHandler<TRequest, TResponse> handler,
                                IEnumerable<IValidator<TRequest>> validators,
                                IHttpContextAccessor httpContextAccessor,
                                IConfigurationService configurationService)
        {
            _handler = handler;
            _configurationService = configurationService;

            if (_handler.GetType().BaseType.Name == typeof(BaseHandler<,>).Name || _handler.GetType().BaseType.Name == typeof(BasePaginationHandler<,>).Name)
            {
                if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    var currentUser = httpContextAccessor.HttpContext.User;
                    if (!string.IsNullOrEmpty(currentUser.FindFirst(JwtRegisteredClaimNames.Jti)?.Value))
                    {
                        var userEmail = currentUser.FindFirst(x => x.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/ShortTypeName"] == JwtRegisteredClaimNames.UniqueName).Value;
                        var userId = Guid.Parse(currentUser.FindFirst(JwtRegisteredClaimNames.Jti).Value);
                        var userRole = currentUser.FindFirst(JwtRegisteredClaimNames.Actort)?.Value;


                        _handler.GetType()
                                .GetProperty(nameof(BaseHandler<BaseRequest<BaseResponse>, BaseResponse>.CurrentUser))
                                .SetValue(_handler, new CurrentUserModel(userId, userEmail, userRole));
                    }
                }
            }
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            TResponse response = default(TResponse);
            var errors = Validate(request);
            if (errors.Any())
                throw new ValidationException(errors);

            response = await _handler.Handle(request, System.Threading.CancellationToken.None);
            if (response.GetType().BaseType.Name == nameof(AuthenticationResponse))
            {
                var authResponse = (response as AuthenticationResponse);
                GenerateAuthToken(authResponse);
                authResponse.Validate();
                response = authResponse as TResponse;
            }

            try
            {
                await (Task)_handler.GetType()
                                .GetMethod(nameof(BaseHandler<BaseRequest<BaseResponse>, BaseResponse>.SaveChanges))
                                .Invoke(_handler, null);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw new CustomException("Cannot store your data in our database. Please try again later. If this error persists, please contact our customer support.");
            }
            return response;
        }


        private IEnumerable<ValidationFailure> Validate(TRequest request)
        {
            ValidatorOptions.LanguageManager.Enabled = true;
            ValidatorOptions.LanguageManager.Culture = Thread.CurrentThread.CurrentCulture;
            var context = new ValidationContext(request);
            return _validators
                .Select(validator => validator.Validate(context))
                .SelectMany(result => result.Errors);
        }

        T GenerateAuthToken<T>(T response)
            where T : AuthenticationResponse
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, response.Username),
                new Claim(JwtRegisteredClaimNames.Jti, response.UserId.ToString())
            };

            var token = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.UtcNow.AddSeconds(30),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VerticalToDoKey")), SecurityAlgorithms.HmacSha256)
                );

            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.ExpiryDate = token.ValidTo;

            return response;
        }
    }
}
