namespace BestPick.Api.Models;

public class DashboardResposta
{
    public int TotalBuscas { get; set; }
    public int TotalFavoritados { get; set; }
    public int TotalAdquiridos { get; set; }
    public List<DashboardBuscaMes> BuscasPorMes { get; set; }
}

public class DashboardBuscaMes
{
    public string Mes { get; set; }
    public int Quantidade { get; set; }
}