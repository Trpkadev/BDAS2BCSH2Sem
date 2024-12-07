using BCSH2BDAS2.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Runtime.CompilerServices;
using static BCSH2BDAS2.Controllers.HomeController;

namespace BCSH2BDAS2.Helpers;

public class TransportationContext(DbContextOptions<TransportationContext> options) : DbContext(options)
{
    public DbSet<DatabazovyObjekt> DatabazoveObjekty { get; set; }
    public DbSet<Garaz> Garaze { get; set; }
    public DbSet<JizdniRad> JizdniRady { get; set; }
    public DbSet<Linka> Linky { get; set; }
    public DbSet<Log> Logy { get; set; }
    public DbSet<Model> Modely { get; set; }
    public DbSet<NakladyNaVozidlo> NakladyNaVozidla { get; set; }
    public DbSet<Pracovnik> Pracovnici { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Schema> Schemata { get; set; }
    public DbSet<Spoj> Spoje { get; set; }
    public DbSet<TarifniPasmo> TarifniPasma { get; set; }
    public DbSet<TypVozidla> TypyVozidel { get; set; }
    public DbSet<Udrzba> Udrzby { get; set; }
    public DbSet<Uzivatel> Uzivatele { get; set; }
    public DbSet<Vozidlo> Vozidla { get; set; }
    public DbSet<Zastavka> Zastavky { get; set; }
    public DbSet<ZaznamTrasy> ZaznamyTras { get; set; }
    public DbSet<Znacka> Znacky { get; set; }
    public DbSet<PracovnikHiearchie> PracovniciHierarchie { get; set; }
    public DbSet<StatistikaLogu> StatistikaLogu { get; set; }
    public DbSet<StatistikaLinky> StatistikaLinek { get; set; }

    #region Procedury

    public async Task CisteniVozidelAsync(bool cleanEveryVehicle)
    {
        const string sql = "PROCEDURY.CISTENI_VOZIDEL(:p_vsechna);";
        await DBPLSQLCallAsync(sql, sqlParams: [new OracleParameter("p_vsechna", cleanEveryVehicle)]);
    }

    public async Task CSVDoZaznamuTrasyAsync(string csv, char separator)
    {
        const string sql = @"   DECLARE
                                    v_csv CLOB;
                                BEGIN
                                    v_csv := :p_csv;
                                    PROCEDURY.CSV_DO_ZAZNAMU_TRASY(v_csv, :p_oddelovac);
                                END;";
        await using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new OracleParameter("p_csv", csv));
        command.Parameters.Add(new OracleParameter("p_oddelovac", separator));
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    public async Task NavyseniPlatuAsync(double multiplier, int minPay)
    {
        const string sql = "PROCEDURY.NAVYSENI_PLATU(:p_multiplier, :p_min_pay);";
        OracleParameter[] sqlParams = [ new("p_multiplier", multiplier),
                                        new("p_min_pay", minPay)];
        await DBPLSQLCallAsync(sql, sqlParams: sqlParams);
    }

    public async Task PlanovaniJrAsync(int idSpoj, TimeOnly from, TimeOnly to, int interval)
    {
        const string sql = "PROCEDURY.PLANOVANI_JR(:p_id_spoj, POM_FCE.CAS_NA_DATE(:p_od), POM_FCE.CAS_NA_DATE(:p_do), :p_interval);";
        OracleParameter[] sqlParams = [ new("p_id_spoj", idSpoj),
                                        new("p_od", from.ToString("t")),
                                        new("p_do", to.ToString("t")),
                                        new("p_interval", interval)];
        await DBPLSQLCallAsync(sql, sqlParams: sqlParams);
    }

    #endregion Procedury

    #region Funkce

    public async Task<string?> GetTabulkaDoCsvAsync(string name, char separator)
    {
        const string sql = @"   DECLARE
                                    v_csv CLOB;
                                BEGIN
                                    SELECT FUNKCE.TABULKA_DO_CSV(:p_nazev, :p_oddelovac) INTO v_csv
                                    FROM DUAL;
                                    :p_result := v_csv;
                                END;";
        OracleParameter[] sqlParams = [ new("p_nazev", name),
                                        new("p_oddelovac", separator)];
        var result = await DBPLSQLCallWithResponseAsync<string>(sql, sqlParams);
        return result;
    }

    public async Task<string?> GetTabulkaDoJsonAsync(string name)
    {
        const string sql = @"  DECLARE
                                   v_json CLOB;
                               BEGIN
                                   SELECT FUNKCE.TABULKA_DO_JSON(:p_nazev) INTO v_json
                                   FROM DUAL;
                                   :p_result := v_json;
                               END;";
        OracleParameter[] sqlParams = [new("p_nazev", name)];
        var result = await DBPLSQLCallWithResponseAsync<string>(sql, sqlParams);
        return result;
    }

    public async Task<double?> GetPrumerneZpozdeni(int idLinka, int pocetDni, int? hodina)
    {
        const string sql = @"  DECLARE
                                    v_result NUMBER;
                                BEGIN
                                    v_result := FUNKCE.PRUM_ZPOZDENI(:id_linka, :pocet_dni, :hodina);
                                    :p_result := v_result_json;
                                END;";
        OracleParameter[] sqlParams = [
            new("id_linka", OracleDbType.Int32, idLinka, ParameterDirection.Input),
            new("pocet_dni", OracleDbType.Int32, pocetDni, ParameterDirection.Input),
            new("hodina", OracleDbType.Int32, hodina, ParameterDirection.Input)
        ];
        // todo udělat lépe
        await using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        var resultParam = new OracleParameter("p_result", OracleDbType.Double, ParameterDirection.Output);
        command.Parameters.AddRange(sqlParams);
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        var result = resultParam.Value;
        await Database.CloseConnectionAsync();
        return (double?)result;
    }

    public async Task<VyhledaniSpojeResponseModel[]?> VyhledaniSpojeAsync(int idZastavkaBegin, int idZastavkaEnd, DateTime timeStart)
    {
        const string sql = @"  DECLARE
                                    v_result_json CLOB;
                                BEGIN
                                    v_result_json := DIJKSTRA.CORE(:p_id_zastavka_from, :p_id_zastavka_to, :p_datetime_start);
                                    :p_result := v_result_json;
                                END;";
        OracleParameter[] sqlParams = [ new("p_id_zastavka_from", OracleDbType.Int32, idZastavkaBegin, ParameterDirection.Input),
                                        new("p_id_zastavka_to", OracleDbType.Int32, idZastavkaEnd, ParameterDirection.Input),
                                        new("p_datetime_start", OracleDbType.Date, timeStart, ParameterDirection.Input)];
        var result = await DBPLSQLCallWithResponseAsync<VyhledaniSpojeResponseModel[]>(sql, sqlParams);
        return result;
    }

    #endregion Funkce

    #region DML procedures

    public async Task DMLGarazeAsync(Garaz garaz)
    {
        string sql = $"{ConvertDMLMethodName()}(:idGaraz, :nazev, :kapacita);";
        OracleParameter[] sqlParams = [ new("idGaraz", ConvertId(garaz.IdGaraz)),
                                        new("nazev", garaz.Nazev),
                                        new("kapacita", garaz.Kapacita)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLJizdni_RadyAsync(JizdniRad jizdniRad)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSpoj, :idZastavka, :casPrijezdu, :casOdjezdu);";
        OracleParameter[] sqlParams = [ new("idSpoj", jizdniRad.IdSpoj),
                                        new("idZastavka", jizdniRad.IdZastavka),
                                        new("casPrijezdu", jizdniRad.CasPrijezdu),
                                        new("casOdjezdu", jizdniRad.CasOdjezdu)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLLinkyAsync(Linka linka)
    {
        string sql = $"{ConvertDMLMethodName()}(:idLinka, :nazev, :typVozidla, :cislo);";
        OracleParameter[] sqlParams = [ new("idLinka", ConvertId(linka.IdLinka)),
                                        new("nazev", linka.Nazev),
                                        new("typVozidla", linka.IdTypVozidla),
                                        new("cislo", linka.Cislo)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLModelyAsync(Model model)
    {
        string sql = $"{ConvertDMLMethodName()}(:idModel, :idTypVozidla, :idZnacka, :nazev, :jeNizkopodlazni);";
        OracleParameter[] sqlParams = [ new("idModel", ConvertId(model.IdModel)),
                                        new("idTypVozidla", model.IdTypVozidla),
                                        new("idZnacka", model.IdZnacka),
                                        new("nazev", model.Nazev),
                                        new("jeNizkopodlazni", model.JeNizkopodlazni)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLPracovniciAsync(Pracovnik pracovnik)
    {
        string sql = $"{ConvertDMLMethodName()}(:idPracovnik, :uzivatelskeJmeno, :heslo, :idNadrizeny, :idRole, :hodinovaMzda, :jmeno, :prijmeni, :telefonniCislo, :email, :rodneCislo, :idUzivatel);";
        OracleParameter[] sqlParams = [ new("idPracovnik", ConvertId(pracovnik.IdPracovnik)),
                                        new("idNadrizeny", pracovnik.IdNadrizeny),
                                        new("idRole", pracovnik.IdNadrizeny),
                                        new("hodinovaMzda", pracovnik.HodinovaMzda),
                                        new("jmeno", pracovnik.Jmeno),
                                        new("prijmeni", pracovnik.Prijmeni),
                                        new("telefonniCislo", pracovnik.TelefonniCislo),
                                        new("email", pracovnik.Email),
                                        new("rodneCislo", pracovnik.RodneCislo),
                                        new("idUzivatel", pracovnik.IdUzivatel)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLRoleAsync(Role role)
    {
        string sql = $"{ConvertDMLMethodName()}(:idRole, :nazev, :prava);";
        OracleParameter[] sqlParams = [ new("idRole", ConvertId(role.IdRole)),
                                        new("nazev", role.Nazev),
                                        new("prava", role.Prava)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLSchemataAsync(Schema schema)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSchema, :nazevSchematu, :nazevSouboru, :typSouboru, :velikostSouboru, :datumZmeny, :soubor, :idUzivatel);";
        OracleParameter[] sqlParams = [ new("idSchema", ConvertId(schema.IdSchema)),
                                        new("nazevSchematu", schema.NazevSchematu),
                                        new("nazevSouboru", schema.NazevSouboru),
                                        new("typSouboru", schema.TypSouboru),
                                        new("velikostSouboru", schema.VelikostSouboru),
                                        new("datumZmeny", schema.DatumZmeny),
                                        new("soubor", schema.Soubor),
                                        new("idUzivatel", schema.IdUzivatel)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLSpojeAsync(Spoj spoj)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSpoj, :idLinka, :garantovaneNizkopodlazni, :jedeVeVsedniDen, :jedeVSobotu, :jedeVNedeli);";
        OracleParameter[] sqlParams = [ new("idSpoj", ConvertId(spoj.IdSpoj)),
                                        new("idLinka", spoj.IdLinka),
                                        new("garantovaneNizkopodlazni", spoj.GarantovaneNizkopodlazni),
                                        new("jedeVeVsedniDen", spoj.JedeVeVsedniDen),
                                        new("jedeVSobotu", spoj.JedeVSobotu ? 1 : 0),
                                        new("jedeVNedeli", spoj.JedeVNedeli)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLTarifni_PasmaAsync(TarifniPasmo tarifniPasmo)
    {
        string sql = $"{ConvertDMLMethodName()}(:idPasmo, :nazev);";
        OracleParameter[] sqlParams = [ new("idPasmo", ConvertId(tarifniPasmo.IdPasmo)),
                                        new("nazev", tarifniPasmo.Nazev)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLTypy_VozidelAsync(TypVozidla typVozidla)
    {
        string sql = $"{ConvertDMLMethodName()}(:idTypVozidla, :nazev);";
        OracleParameter[] sqlParams = [ new("idTypVozidla", ConvertId(typVozidla.IdTypVozidla)),
                                        new("nazev", typVozidla.Nazev)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLUdrzbyAsync(Udrzba udrzba)
    {
        string sql = $"{ConvertDMLMethodName()}(:idUdrzba, :idVozidlo, :datum, :konecUdrzby, :popisUkonu, :cena, :umytoVMycce, :cistenoOzonem, :typUdrzby);";
        OracleParameter[] sqlParams = [ new ("idUdrzba", ConvertId(udrzba.IdUdrzba)),
                                        new ("idVozidlo", udrzba.IdVozidlo),
                                        new ("datum", OracleDbType.Date, udrzba.Datum, ParameterDirection.Input),
                                        new ("konecUdrzby", OracleDbType.Date, udrzba.KonecUdrzby, ParameterDirection.Input),
                                        new ("popisUkonu",null),
                                        new ("cena", null),
                                        new ("umytoVMycce",  null),
                                        new ("cistenoOzonem", null),
                                        new ("typUdrzby", OracleDbType.Char, udrzba.TypUdrzby, ParameterDirection.Input)];
        switch (udrzba)
        {
            case Oprava oprava:
                sqlParams.First(p => p.ParameterName == "popisUkonu").Value = oprava.PopisUkonu;
                sqlParams.First(p => p.ParameterName == "cena").Value = oprava.Cena;
                break;

            case Cisteni cisteni:
                sqlParams.First(p => p.ParameterName == "umytoVMycce").Value = cisteni.UmytoVMycce;
                sqlParams.First(p => p.ParameterName == "cistenoOzonem").Value = cisteni.CistenoOzonem;
                break;
        }
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLUzivateleAsync(Uzivatel uzivatel)
    {
        string sql = $"{ConvertDMLMethodName()}(:idUzivatel, :jmeno, :heslo, :role);";
        OracleParameter[] sqlParams = [ new("idUzivatel", ConvertId(uzivatel.IdUzivatel)),
                                        new("jmeno", uzivatel.UzivatelskeJmeno),
                                        new("heslo", uzivatel.Heslo),
                                        new("role", uzivatel.IdRole)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLVozidlaAsync(Vozidlo vozidlo)
    {
        string sql = $"{ConvertDMLMethodName()}(:idVozidlo,:rokVyroby, :najeteKilometry, :kapacita, :maKlimatizaci, :idGaraz, :idModel);";
        OracleParameter[] sqlParams = [ new("idVozidlo", ConvertId(vozidlo.IdVozidlo)),
                                        new("rokVyroby", vozidlo.RokVyroby),
                                        new("najeteKilometry", vozidlo.NajeteKilometry),
                                        new("kapacita", vozidlo.Kapacita),
                                        new("maKlimatizaci", vozidlo.MaKlimatizaci ? 1 : 0),
                                        new("idGaraz", vozidlo.IdGaraz),
                                        new("idModel", vozidlo.IdModel)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLZastavkyAsync(Zastavka zastavka)
    {
        string sql = $"{ConvertDMLMethodName()}(:idZastavka, :nazev, :souradniceX, :souradniceY, :idPasmo);";
        OracleParameter[] sqlParams = [ new("idZastavka", ConvertId(zastavka.IdZastavka)),
                                        new("nazev", zastavka.Nazev),
                                        new("souradniceX", zastavka.SouradniceX),
                                        new("souradniceY", zastavka.SouradniceY),
                                        new("idPasmo", zastavka.IdPasmo)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLZaznamy_TrasyAsync(ZaznamTrasy zaznamTrasy)
    {
        string sql = $"{ConvertDMLMethodName()}(:idZaznam, :idSpoj, :idZastavka, :idVozidlo, :casPrijezdu, :casOdjezdu);";
        OracleParameter[] sqlParams = [ new("idZaznam", ConvertId(zaznamTrasy.IdZaznam)),
                                        new("idJizdniRad", zaznamTrasy.IdJizniRad),
                                        new("idVozidlo", zaznamTrasy.IdVozidlo),
                                        new("casPrijezdu", zaznamTrasy.CasPrijezdu),
                                        new("casOdjezdu", zaznamTrasy.CasOdjezdu)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLZnackyAsync(Znacka znacka)
    {
        string sql = $"{ConvertDMLMethodName()}(:ídZnacka, :nazev);";
        OracleParameter[] sqlParams = [ new("ídZnacka", ConvertId(znacka.IdZnacka)),
                                        new("nazev", znacka.Nazev)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    #endregion DML procedures

    #region views

    public async Task<List<StatistikaLinky>> GetLinkyStatistikaAsync()
    {
        return await StatistikaLinek.FromSqlRaw("SELECT * FROM LINKY_STAT").ToListAsync();
    }

    public async Task<List<StatistikaLogu>> GetLogyStatistikaAsync()
    {
        return await StatistikaLogu.FromSqlRaw("SELECT * FROM LOGY_STAT").ToListAsync();
    }

    public async Task<List<PracovnikHiearchie>> GetPracovniciHierarchieAsync()
    {
        return await PracovniciHierarchie.FromSqlRaw("SELECT * FROM PRACOVNICI_HIERARCHIE").ToListAsync();
    }

    public async Task<List<DatabazovyObjekt>> GetDBObjektyAsync()
    {
        return await DatabazoveObjekty.FromSqlRaw("SELECT * FROM DB_OBJEKTY").ToListAsync();
    }

    public async Task<Garaz?> GetGarazByIdAsync(int idGaraz)
    {
        return await Garaze.FromSqlRaw("SELECT * FROM GARAZE_VIEW WHERE ID_GARAZ = {0}", idGaraz).FirstOrDefaultAsync();
    }

    public async Task<List<Garaz>> GetGarazeAsync()
    {
        return await Garaze.FromSqlRaw("SELECT * FROM GARAZE_VIEW").ToListAsync();
    }

    public async Task<JizdniRad?> GetJizdniRadByIdAsync(int idJizdniRad)
    {
        return await JizdniRady.FromSqlRaw("SELECT * FROM JIZDNI_RADY_VIEW WHERE ID_JIZDNI_RAD = {0}", idJizdniRad).FirstOrDefaultAsync();
    }

    public async Task<List<JizdniRad>> GetJizdniRadyAsync()
    {
        return await JizdniRady.FromSqlRaw("SELECT * FROM JIZDNI_RADY_VIEW").ToListAsync();
    }

    public async Task<Linka?> GetLinkaByIdAsync(int idLinka)
    {
        return await Linky.FromSqlRaw("SELECT * FROM LINKY_VIEW WHERE ID_LINKA = {0}", idLinka).FirstOrDefaultAsync();
    }

    public async Task<List<Linka>> GetLinkyAsync()
    {
        return await Linky.FromSqlRaw("SELECT * FROM LINKY_VIEW").ToListAsync();
    }

    public async Task<List<Log>> GetLogyAsync()
    {
        return await Logy.FromSqlRaw("SELECT * FROM LOGY_VIEW").ToListAsync();
    }

    public async Task<Model?> GetModelByIdAsync(int idModel)
    {
        return await Modely.FromSqlRaw("SELECT * FROM MODELY_VIEW WHERE ID_MODEL = {0}", idModel).FirstOrDefaultAsync();
    }

    public async Task<List<Model>> GetModelyAsync()
    {
        return await Modely.FromSqlRaw("SELECT * FROM MODELY_VIEW").ToListAsync();
    }

    public async Task<List<NakladyNaVozidlo>> GetNakladyNaVozidla()
    {
        return await NakladyNaVozidla.FromSqlRaw("SELECT * FROM NAKLADY_VOZIDLA").ToListAsync();
    }

    public async Task<List<Pracovnik>> GetPracovniciAsync()
    {
        return await Pracovnici.FromSqlRaw("SELECT * FROM PRACOVNICI_VIEW").ToListAsync();
    }

    public async Task<Pracovnik?> GetPracovnikByIdAsync(int idPracovnik)
    {
        return await Pracovnici.FromSqlRaw("SELECT * FROM PRACOVNICI_VIEW WHERE ID_PRACOVNIK = {0}", idPracovnik).FirstOrDefaultAsync();
    }

    public async Task<Pracovnik?> GetPracovnikByUserIdAsync(int idUzivatel)
    {
        return await Pracovnici.FromSqlRaw("SELECT * FROM PRACOVNICI_VIEW WHERE ID_UZIVATEL = {0}", idUzivatel).FirstOrDefaultAsync();
    }

    public async Task<List<Role>> GetRoleAsync()
    {
        return await Role.FromSqlRaw("SELECT * FROM ROLE_VIEW").ToListAsync();
    }

    public async Task<Role?> GetRoleByIdAsync(int idRole)
    {
        return await Role.FromSqlRaw("SELECT * FROM ROLE_VIEW WHERE ID_ROLE = {0}", idRole).FirstOrDefaultAsync();
    }

    public async Task<Schema?> GetSchemaByIdAsync(int idSchema)
    {
        return await Schemata.FromSqlRaw("SELECT * FROM SCHEMATA_VIEW WHERE ID_SCHEMA = {0}", idSchema).FirstOrDefaultAsync();
    }

    public async Task<List<Schema>> GetSchemataAsync()
    {
        return await Schemata.FromSqlRaw("SELECT * FROM SCHEMATA_VIEW").ToListAsync();
    }

    public async Task<Spoj?> GetSpojByIdAsync(int idSpoj)
    {
        return await Spoje.FromSqlRaw("SELECT * FROM SPOJE_VIEW WHERE ID_SPOJ = {0}", idSpoj).FirstOrDefaultAsync();
    }

    public async Task<List<Spoj>> GetSpojeAsync()
    {
        return await Spoje.FromSqlRaw("SELECT * FROM SPOJE_VIEW").ToListAsync();
    }

    public async Task<List<TarifniPasmo>> GetTarifni_PasmaAsync()
    {
        return await TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA_VIEW").ToListAsync();
    }

    public async Task<TarifniPasmo?> GetTarifniPasmoByIdAsync(int idTarifniPasmo)
    {
        return await TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA_VIEW WHERE ID_PASMO = {0}", idTarifniPasmo).FirstOrDefaultAsync();
    }

    public async Task<TypVozidla?> GetTyp_VozidlaByIdAsync(int idTypVozidla)
    {
        return await TypyVozidel.FromSqlRaw("SELECT * FROM TYPY_VOZIDEL_VIEW WHERE ID_TYP_VOZIDLA = {0}", idTypVozidla).FirstOrDefaultAsync();
    }

    public async Task<List<TypVozidla>> GetTypy_VozidelAsync()
    {
        return await TypyVozidel.FromSqlRaw("SELECT * FROM TYPY_VOZIDEL_VIEW").ToListAsync();
    }

    public async Task<Udrzba?> GetUdrzbaByIdAsync(int idUdrzba)
    {
        return await Udrzby.FromSqlRaw("SELECT * FROM UDRZBY_VIEW WHERE ID_UDRZBA = {0}", idUdrzba).FirstOrDefaultAsync();
    }

    public async Task<List<Udrzba>> GetUdrzbyAsync()
    {
        return await Udrzby.FromSqlRaw("SELECT * FROM UDRZBY_VIEW").ToListAsync();
    }

    public async Task<Uzivatel?> GetUzivatelByIdAsync(int idUzivatel)
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW WHERE ID_UZIVATEL = {0}", idUzivatel).FirstOrDefaultAsync();
    }

    public async Task<Uzivatel?> GetUzivatelByNamePwdAsync(string name, string pwdHash)
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW WHERE UZIVATELSKE_JMENO = {0} AND HESLO = {1}", name, pwdHash).FirstOrDefaultAsync();
    }

    public async Task<List<Uzivatel>> GetUzivateleAsync()
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW").ToListAsync();
    }

    public async Task<bool> GetUzivatelUsernameExistsAsync(string name)
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW WHERE UZIVATELSKE_JMENO = {0}", name).FirstOrDefaultAsync() != null;
    }

    public async Task<List<Vozidlo>> GetVozidlaAsync()
    {
        return await Vozidla.FromSqlRaw("SELECT * FROM VOZIDLA_VIEW").ToListAsync();
    }

    public async Task<Vozidlo?> GetVozidloByIdAsync(int idVozidlo)
    {
        return await Vozidla.FromSqlRaw("SELECT * FROM VOZIDLA_VIEW WHERE ID_VOZIDLO = {0}", idVozidlo).FirstOrDefaultAsync();
    }

    public async Task<Zastavka?> GetZastavkaByIdAsync(int idZastavka)
    {
        return await Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY_VIEW WHERE ID_ZASTAVKA = {0}", idZastavka).FirstOrDefaultAsync();
    }

    public async Task<List<Zastavka>> GetZastavkyAsync()
    {
        return await Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY_VIEW").ToListAsync();
    }

    public async Task<ZaznamTrasy?> GetZaznam_TrasyByIdAsync(int idZaznamTrasy)
    {
        return await ZaznamyTras.FromSqlRaw("SELECT * FROM ZAZNAMY_TRASY_VIEW WHERE ID_ZAZNAM = {0}", idZaznamTrasy).FirstOrDefaultAsync();
    }

    public async Task<List<ZaznamTrasy>> GetZaznamy_TrasyAsync()
    {
        return await ZaznamyTras.FromSqlRaw("SELECT * FROM ZAZNAMY_TRASY_VIEW").ToListAsync();
    }

    public async Task<Znacka?> GetZnackaByIdAsync(int idZnacka)
    {
        return await Znacky.FromSqlRaw("SELECT * FROM ZNACKY_VIEW WHERE ID_ZNACKA = {0}", idZnacka).FirstOrDefaultAsync();
    }

    public async Task<List<Znacka>> GetZnackyAsync()
    {
        return await Znacky.FromSqlRaw("SELECT * FROM ZNACKY_VIEW").ToListAsync();
    }

    #endregion views

    #region EF Core config

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
#if DEBUG
        modelBuilder.HasDefaultSchema("ST69642");
#elif RELEASE
        modelBuilder.HasDefaultSchema("ST69612");
#endif

        modelBuilder.Entity<DatabazovyObjekt>().HasNoKey();
        modelBuilder.Entity<NakladyNaVozidlo>().HasNoKey();
        modelBuilder.Entity<PracovnikHiearchie>().HasNoKey();
        modelBuilder.Entity<StatistikaLogu>().HasNoKey();
        modelBuilder.Entity<Udrzba>()
        .HasDiscriminator<char>("TYP_UDRZBY")
        .HasValue<Cisteni>('c')
        .HasValue<Oprava>('o')
        .HasValue<Udrzba>('x');
    }

    #endregion EF Core config

    #region Helper methods

    public async Task DeleteFromTableAsync(string table, (string paramName, object param)[] values)
    {
        using var command = Database.GetDbConnection().CreateCommand();
        for (int i = 0; i < values.Length; i++)
        {
            if (i == 0)
                command.CommandText = $"DELETE FROM {table} WHERE {values[i].paramName} = :Param";
            else
                command.CommandText += $"AND {values[i].paramName} = :Param";
            command.Parameters.Add(new OracleParameter($":Param", values[i].param));
        }
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    private static string ConvertDMLMethodName([CallerMemberName] string methodName = "") => methodName.ToUpper().Replace("DML", "DML_").Replace("ASYNC", string.Empty);

    private static int? ConvertId(int id) => id == 0 ? null : id;

    private async Task DBPLSQLCallAsync(string sql, OracleParameter[]? sqlParams = null)
    {
        var connection = Database.GetDbConnection();

        using var command = connection.CreateCommand();
        command.CommandText = $@"   BEGIN
                                    {sql}
                                    END;";
        if (sqlParams != null)
            command.Parameters.AddRange(sqlParams);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }

    private async Task<T?> DBPLSQLCallWithResponseAsync<T>(string sql, OracleParameter[]? sqlParams = null) where T : class
    {
        try
        {
            var connection = Database.GetDbConnection();
            var resultParam = new OracleParameter("p_result", OracleDbType.Clob, ParameterDirection.Output);
            string? resultJson = null;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                if (sqlParams != null)
                    command.Parameters.AddRange(sqlParams);
                command.Parameters.Add(resultParam);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                if (resultParam.Value != DBNull.Value && !((OracleClob)resultParam.Value).IsNull)
                    resultJson = ((OracleClob)resultParam.Value).Value;
                await connection.CloseAsync();
            }
            return resultJson == null ? null : JsonConvert.DeserializeObject<T>(resultJson);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task DMLPackageCallAsync(string sql, OracleParameter[] sqlParams)
    {
        using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = $@"   BEGIN
                                    DML_PROCEDURY.{sql}
                                    END;";
        command.CommandType = CommandType.Text;
        command.Parameters.AddRange(sqlParams);
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    #endregion Helper methods
}