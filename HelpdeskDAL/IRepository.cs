/**
 * File name:	    IRepository.cs
 * Purpose: 		Defines a standard contract for generic data access operations, including retrieval, 
 *                          addition, update, and deletion of entities.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using System.Linq.Expressions;

namespace HelpdeskDAL
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetSome(Expression<Func<T, bool>> match);
        Task<T?> GetOne(Expression<Func<T, bool>> match);
        Task<T> Add(T entity);
        Task<UpdateStatus> Update(T enity);
        Task<int> Delete(int i);
    }
}
