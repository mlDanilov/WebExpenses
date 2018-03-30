using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainExpenses.Abstract.Repositories
{
    /// <summary>
    /// Базовый интерфейс для репозиториев
    /// (Single Responsibility Principle)
    /// </summary>
    public interface IEntityRepositiory<TInput, TResult>  
        where TInput: class
        where TResult : class

    {

        IQueryable<TResult> Entities { get; }

        TResult Create(TInput entity_);

        void Update(TInput entity_);

        void Delete(TInput entity_);

    }
}
