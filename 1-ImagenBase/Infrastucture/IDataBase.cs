using System.Collections.Generic;
using System.Threading.Tasks;

namespace ms_base
{
    public interface IDataBase
    {
        Task<TimeDto> AddTimeAsync(TimeDto time);
        Task<List<TimeDto>> GetAllAsync();
    }
}
