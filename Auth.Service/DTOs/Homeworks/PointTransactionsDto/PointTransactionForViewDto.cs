namespace Auth.Service.DTOs.Homeworks.PointTransactionsDto;

public class PointTransactionForViewDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int PointsChanged { get; set; }
    public string Reason { get; set; }
    public DateTime CreatedAt { get; set; }
}
