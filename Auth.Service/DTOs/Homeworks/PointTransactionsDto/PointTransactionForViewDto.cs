namespace Auth.Service.DTOs.Homeworks.PointTransactionsDto;

public class PointTransactionForViewDto
{
    public long Id { get; set; }
    public string UserFullName { get; set; }
    public int Points { get; set; }
    public string Reason { get; set; }
}
