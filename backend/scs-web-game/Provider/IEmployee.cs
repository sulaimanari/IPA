using scs_web_game.DTOs.Employee;

namespace scs_web_game.Provider
{
    public interface IEmployee
    {
        public Task<RandomEmployeeAndNameListDto> GetRandomEmployeeAndNameList();
    }
}
