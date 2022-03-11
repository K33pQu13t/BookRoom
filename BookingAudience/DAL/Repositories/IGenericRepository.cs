using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.DAL.Repositories
{
    public interface IGenericRepository<T>
    {
        /// <summary>
        /// получить все записи
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Get();
        /// <summary>
        /// получить запись
        /// </summary>
        /// <param name="id">идентификатор нужной записи</param>
        /// <returns>запись с указанным идентификатором</returns>
        T Get(int id);
        /// <summary>
        /// получить запись
        /// </summary>
        /// <param name="id">идентификатор нужной записи</param>
        /// <returns>запись с указанным идентификатором</returns>
        Task<T> GetAsync(int id);
        /// <summary>
        /// добавляет новую запись
        /// </summary>
        /// <param name="item">объект для записи</param>
        void Create(T item);
        /// <summary>
        /// добавляет новую запись
        /// </summary>
        /// <param name="item">добавленная запись</param>
        Task<T> CreateAsync(T item);
        /// <summary>
        /// обновляет данные записи
        /// </summary>
        /// <param name="updatedItem">обновлённый объект</param>
        void Update(T updatedItem);
        /// <summary>
        /// удалить запись в БД по идентификатору
        /// </summary>
        /// <param name="id">идентификатор записи</param>
        /// <returns>возвращает удалённый объект</returns>
        Task<T> DeleteAsync(int id);
        void DeleteAll();
    }
}
