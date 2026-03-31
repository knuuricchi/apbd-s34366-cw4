using System.Data.Common;
using LinqConsoleLab.PL.Data;
using LinqConsoleLab.PL.Models;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        var query = 
            from s in DaneUczelni.Studenci
            where s.Miasto.Equals("Warsaw")
            select $"{s.NumerIndeksu}, {s.Imie}, {s.Nazwisko}, {s.Miasto}";
        
        return query;
    }
    
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Email);
    }
    
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        var query = 
            from s in DaneUczelni.Studenci
            orderby s.Nazwisko ascending
            select $"{s.NumerIndeksu}, {s.Imie}, {s.Nazwisko}, {s.Miasto}";
        
        return query;
    }
    
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var query = 
            (
                from p in DaneUczelni.Przedmioty
                where p.Kategoria == "Analytics"
                orderby p.DataStartu
                select p
                )
            .FirstOrDefault();

        if (query is null)
            return new[] { "Brak przedmiotu z kategorii Analytics." };

        return new[]
        {
            $"{query.Nazwa}"
        };
    }

    
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var res = DaneUczelni.Zapisy.Any(e => e.CzyAktywny) ? "Yes" : "No";
        return [res];
    }
    
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        bool czyKazdyMaKatedre =
            DaneUczelni.Prowadzacy.All(p => !string.IsNullOrEmpty(p.Katedra)
            );
        return new[]
        {
            czyKazdyMaKatedre ? "No" : "Yes",
        };
    }
    
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var query =
            DaneUczelni.Zapisy
                .Count(z => z.CzyAktywny.Equals(true))
                .ToString();
        return [query];
    }
    
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        var query = (from s in DaneUczelni.Studenci
                select s.Miasto)
            .Distinct()
            .OrderBy(m => m)
            .ToList();
        
        return query;
    }
    
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu:yyyy-MM-dd}, StudentId: {z.StudentId}, PrzedmiotId: {z.PrzedmiotId}");
    }
    
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa}, {p.Kategoria}");
    }
    
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        var query =
            from s in DaneUczelni.Studenci
            join z in DaneUczelni.Zapisy on s.Id equals z.StudentId
            select $"{s.Imie} {s.Nazwisko}, Data zapisu: {z.DataZapisu:yyyy-MM-dd}";

        return query;
    }
    
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
            var query =
                from z in DaneUczelni.Zapisy
                join s in DaneUczelni.Studenci on z.StudentId equals s.Id
                join p in DaneUczelni.Przedmioty on z.PrzedmiotId equals p.Id
                select $"{s.Imie} {s.Nazwisko} - {p.Nazwa}";
            
            return query;
    }
    
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        var query = from z in DaneUczelni.Zapisy
            join p in DaneUczelni.Przedmioty on z.PrzedmiotId equals p.Id
            group z by p.Nazwa into g
            select $"{g.Key}: {g.Count()}";

        return query;
    }
    
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        var query = from z in DaneUczelni.Zapisy
            where z.OcenaKoncowa != null
            join p in DaneUczelni.Przedmioty on z.PrzedmiotId equals p.Id
            group z by p.Nazwa into g
            select $"{g.Key}: {g.Average(x => x.OcenaKoncowa):F2}";

        return query;
    }
    
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        var query = from z in DaneUczelni.Zapisy
            join p in DaneUczelni.Przedmioty on z.PrzedmiotId equals p.Id
            group z by p.Nazwa into g
            select $"{g.Key}: {g.Count()}";

        return query;
    }
    
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        var query = from s in DaneUczelni.Studenci
            join z in DaneUczelni.Zapisy on s.Id equals z.StudentId
            where z.OcenaKoncowa.HasValue
            group z by new { s.Imie, s.Nazwisko } into g
            select $"{g.Key.Imie} {g.Key.Nazwisko}: {g.Max(x => x.OcenaKoncowa)}";
        
        return query;
    }
    
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        var query = from s in DaneUczelni.Studenci
            join z in DaneUczelni.Zapisy on s.Id equals z.StudentId
            where z.CzyAktywny
            group z by new { s.Imie, s.Nazwisko } into g
            where g.Count() > 1
            select $"{g.Key.Imie} {g.Key.Nazwisko}: {g.Count()} aktywnych przedmiotow";
        
        return query;
    }
    
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        var query = from p in DaneUczelni.Przedmioty
            where p.DataStartu.Month == 4 && p.DataStartu.Year == 2026
            join z in DaneUczelni.Zapisy on p.Id equals z.PrzedmiotId into zapisyGroup
            where zapisyGroup.All(z => z.OcenaKoncowa == null)
            select p.Nazwa;
        
        return query;
    }
    
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        var query = from pr in DaneUczelni.Prowadzacy
            join p in DaneUczelni.Przedmioty on pr.Id equals p.ProwadzacyId into przedmiotyGroup
            from p in przedmiotyGroup.DefaultIfEmpty()
            join z in DaneUczelni.Zapisy on p.Id equals z.PrzedmiotId into zapisyGroup
            from z in zapisyGroup.DefaultIfEmpty()
            where z.OcenaKoncowa != null
            group z by new { pr.Imie, pr.Nazwisko } into g
            select $"{g.Key.Imie} {g.Key.Nazwisko}: {(g.Average(x => x.OcenaKoncowa) == 0 ? "Brak ocen" : g.Average(x => x.OcenaKoncowa).ToString())}";

        return query;
    }
    
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        var query = from s in DaneUczelni.Studenci
            join z in DaneUczelni.Zapisy on s.Id equals z.StudentId
            where z.CzyAktywny
            group z by s.Miasto into g
            orderby g.Count() descending
            select $"{g.Key}: {g.Count()} aktywnych zapisoww";

        return query;
    }
}
