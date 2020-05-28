﻿using Lamar;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Abstractions;
using VerticalToDo.Abstractions.Exceptions;
using VerticalToDo.Core;

namespace VerticalToDo.Services
{
    /// <summary>
    /// Abstract handler responsible to provide support for inherited handlers
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
             where TRequest : BaseRequest<TResponse>
             where TResponse : BaseResponse
    {
        private VerticalToDoDbContext _dbContext;
        private IConfigurationService configsService;
        private CurrentUserModel _currentLoggedInUser;

        [SetterProperty]
        public VerticalToDoDbContext DbContext
        {
            set
            {
                if (_dbContext != null)
                    throw new CustomException("DbContext has already beeen initialized!");
                else
                    _dbContext = value;
            }
        }

        [SetterProperty]
        public IConfigurationService ConfigsService
        {
            set { configsService = value; }
        }

        public CurrentUserModel CurrentUser
        {
            set { _currentLoggedInUser = value; }
        }

        /// <summary>
        /// Use this when you want to do CRUD on the database using Entity Framework
        /// </summary>
        protected VerticalToDoDbContext _ef => _dbContext;

        /// <summary>
        /// Use this when you want to do CRUD on the database using Dapper
        /// </summary>
        protected IDbConnection _connection => new SqlConnection(configsService.ConnectionString);

        /// <summary>
        /// Use this whenever you need to get configs from the AppSettings section in the appSettings.json configuration
        /// </summary>
        protected IConfigurationService _configsService => configsService;

        /// <summary>
        /// Use this when you need info on the logged in user which called the API. It contains its Id and e-mail address
        /// </summary>
        protected CurrentUserModel _currentUser => _currentLoggedInUser;

        /// <summary>
        /// DO NOT INVOKE IN HANDLERS. It's auto-invoked when a handler finishes it's job.
        /// </summary>
        /// <returns></returns>
        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an already existing entity
        /// </summary>
        /// <param name="entity">The entity which needs to be updated in the database</param>
        protected void Update(BaseEntity entity)
        {
            if (entity.CreatedBy.Equals(Guid.Empty))
            {
                entity.CreatedBy = _currentUser?.Id ?? Guid.Empty;
                entity.CreatedOn = DateTimeOffset.UtcNow;
            }
            entity.LastModifiedOn = DateTimeOffset.UtcNow;
            entity.LastModifiedBy = _currentUser?.Id ?? Guid.Empty;

            if (entity.Id.Equals(Guid.Empty))
                _ef.Add(entity);
            else
                _ef.Update(entity);
        }

        /// <summary>
        /// Query a db set
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : BaseEntity
        {
            return _ef.Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// Adds an entity to the database, and returns its Id
        /// </summary>
        /// <param name="entity">The entity to add in the database</param>
        /// <returns>The Id generated by the database for the added entity</returns>
        protected Guid Add(BaseEntity entity)
        {
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = _currentUser?.Id ?? Guid.Empty;
            entity.LastModifiedOn = entity.CreatedOn;
            entity.LastModifiedBy = entity.CreatedBy;

            return entity.Id = _ef.Add(entity).Entity.Id;
        }

        protected void Add<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            foreach (var item in entities)
            {
                item.CreatedOn = DateTime.UtcNow;
                item.CreatedBy = _currentUser?.Id ?? Guid.Empty;
                item.LastModifiedOn = item.CreatedOn;
                item.LastModifiedBy = item.CreatedBy;
            }

            _ef.AddRange(entities);
        }

        /// <summary>
        /// HARD deletes the given entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be HARD deleted</param>
        protected void Remove(BaseEntity entity)
        {
            entity.LastModifiedOn = DateTime.UtcNow;
            entity.LastModifiedBy = _currentUser?.Id ?? Guid.Empty;

            _ef.Remove(entity);
        }

        /// <summary>
        /// HARD deletes the given entity from the database.
        /// </summary>
        /// <param name="listOfEntities">The entity to be HARD deleted</param>
        protected void Remove<T>(IEnumerable<T> listOfEntities) where T : BaseEntity
        {
            listOfEntities.ToList().ForEach(x =>
            {
                x.LastModifiedOn = DateTime.UtcNow;
                x.LastModifiedBy = _currentUser?.Id ?? Guid.Empty;
            });

            _ef.RemoveRange(listOfEntities);
        }

        /// <summary>
        /// Returns an entity of type <typeparamref name="EntityType"/> which matches the given Id parameter
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        /// <param name="id">Id of the entity to retrive</param>
        /// <returns>Found entity of type <typeparamref name="EntityType"/> if exists, otherwise null</returns>
        protected async Task<EntityType> GetById<EntityType>(Guid id)
            where EntityType : BaseEntity
        {
            return await _ef.FindAsync<EntityType>(id);
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public class CurrentUserModel
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Role { get; set; }

        public CurrentUserModel(Guid id, string email, string role)
        {
            Id = id;
            Email = email;
            Role = role;
        }
    }
}