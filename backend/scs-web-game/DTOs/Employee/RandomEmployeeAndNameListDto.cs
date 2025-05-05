namespace scs_web_game.DTOs.Employee
{
    public class RandomEmployeeAndNameListDto
    {
        public Guid EmployeeId { get; set; }
        public required List<EmployeeNameDto> RandomNameList { get; set; }
        public required string CorrectName { get; set; }
        public required byte[] Image { get; set; }
    }
}
