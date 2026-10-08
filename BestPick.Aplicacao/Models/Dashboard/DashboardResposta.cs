namespace BestPick.Api.Models;

public class DashboardResposta
{
    public int TotalBuscas { get; set; }
    public int TotalFavoritados { get; set; }
    public int TotalAdquiridos { get; set; }
    public decimal PorcentagemBuscas { get; set; }
    public decimal PorcentagemFavoritados { get; set; }
    public decimal PorcentagemAdquiridos { get; set; }
}
