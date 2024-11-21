using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace BCSH2BDAS2.Models;

public class TransportationContext(DbContextOptions<TransportationContext> options) : DbContext(options)
{
    public DbSet<Cisteni> Cisteni { get; set; }
    public DbSet<Garaz> Garaze { get; set; }
    public DbSet<JizniRad> JizdniRady { get; set; }
    public DbSet<Linka> Linky { get; set; }
    public DbSet<Model> Modely { get; set; }
    public DbSet<Oprava> Opravy { get; set; }
    public DbSet<Spoj> Spoje { get; set; }
    public DbSet<TarifniPasmo> TarifniPasma { get; set; }
    public DbSet<TypVozidla> TypyVozidel { get; set; }
    public DbSet<Udrzba> Udrzby { get; set; }
    public DbSet<Uzivatel> Uzivatele { get; set; }
    public DbSet<Vozidlo> Vozidla { get; set; }
    public DbSet<Zastavka> Zastavky { get; set; }
    public DbSet<ZaznamTrasy> ZaznamyTras { get; set; }
    public DbSet<Znacka> Znacky { get; set; }

    public async Task DMLGarazeAsync(Garaz garaz)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idGaraz, :nazev, :kapacita);";
        OracleParameter[] sqlParams = [ new OracleParameter("idGaraz", ConvertId(garaz.IdGaraz)),
                                        new OracleParameter("nazev", garaz.Nazev),
                                        new OracleParameter("kapacita", garaz.Kapacita)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLJizdni_RadyAsync(JizniRad jizdniRad)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idSpoj, :idZastavka, :casPrijezdu, :casOdjezdu);";
        OracleParameter[] sqlParams = [ new OracleParameter("idSpoj", jizdniRad.IdSpoj),
                                        new OracleParameter("idZastavka", jizdniRad.IdZastavka),
                                        new OracleParameter("casPrijezdu", jizdniRad.CasPrijezdu),
                                        new OracleParameter("casOdjezdu", jizdniRad.CasOdjezdu)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLLinkyAsync(Linka linka)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idLinka, :nazev, :typVozidla, :cislo);";
        OracleParameter[] sqlParams = [ new OracleParameter("idLinka", ConvertId(linka.IdLinka)),
                                        new OracleParameter("nazev", linka.Nazev),
                                        new OracleParameter("typVozidla", linka.IdTypVozidla),
                                        new OracleParameter("cislo", linka.Cislo)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLModelyAsync(Model model)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idModel, :idTypVozidla, :idZnacka, :nazev, :jeNizkopodlazni);";
        OracleParameter[] sqlParams = [ new OracleParameter("idModel", ConvertId(model.IdModel)),
                                        new OracleParameter("idTypVozidla", model.IdTypVozidla),
                                        new OracleParameter("idZnacka", model.IdZnacka),
                                        new OracleParameter("nazev", model.Nazev),
                                        new OracleParameter("jeNizkopodlazni", model.JeNizkopodlazni)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLRoleAsync(Role role)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idRole, :nazev, :prava);";
        OracleParameter[] sqlParams = [ new OracleParameter("idRole", ConvertId(role.IdRole)),
                                        new OracleParameter("nazev", role.Nazev),
                                        new OracleParameter("prava", role.Prava)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLSchemataAsync(Schema schema)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idSchema, :nazevSchematu, :nazevSouboru, :typSouboru, :velikostSouboru, :datumZmeny, :soubor);";
        OracleParameter[] sqlParams = [ new OracleParameter("idSchema", ConvertId(schema.IdSchema)),
                                        new OracleParameter("nazevSchematu", schema.NazevSchematu),
                                        new OracleParameter("nazevSouboru", schema.NazevSouboru),
                                        new OracleParameter("typSouboru", schema.TypSouboru),
                                        new OracleParameter("velikostSouboru", schema.VelikostSouboru),
                                        new OracleParameter("datumZmeny", schema.DatumZmeny),
                                        new OracleParameter("soubor", schema.Soubor)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLSpojeAsync(Spoj spoj)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idSpoj, :idLinka, :garantovaneNizkopodlazni, :jedeVeVsedniDen, :jedeVSobotu, :jedeVNedeli);";
        OracleParameter[] sqlParams = [ new OracleParameter("idSpoj", ConvertId(spoj.IdSpoj)),
                                        new OracleParameter("idLinka", spoj.IdLinka),
                                        new OracleParameter("garantovaneNizkopodlazni", spoj.GarantovaneNizkopodlazni),
                                        new OracleParameter("jedeVeVsedniDen", spoj.JedeVeVsedniDen),
                                        new OracleParameter("jedeVSobotu", spoj.JedeVSobotu ? 1 : 0),
                                        new OracleParameter("jedeVNedeli", spoj.JedeVNedeli)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLTarifni_PasmaAsync(TarifniPasmo tarifniPasmo)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idPasmo, :nazev);";
        OracleParameter[] sqlParams = [ new OracleParameter("idPasmo", ConvertId(tarifniPasmo.IdPasmo)),
                                        new OracleParameter("nazev", tarifniPasmo.Nazev)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLTypy_VozidelAsync(TypVozidla typVozidla)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idTypVozidla, :nazev);";
        OracleParameter[] sqlParams = [ new OracleParameter("idTypVozidla", ConvertId(typVozidla.IdTypVozidla)),
                                        new OracleParameter("nazev", typVozidla.Nazev)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLUdrzbyAsync(Udrzba udrzba)
    {
        List<OracleParameter> sqlParams = [ new OracleParameter("idUdrzba", ConvertId(udrzba.IdUdrzba)),
                                            new OracleParameter("idVozidlo", udrzba.IdVozidlo),
                                            new OracleParameter("datum", udrzba.Datum)];
        StringBuilder sql = new($"{ConvertMethodNameToDML()}(:idUdrzba, :idVozidlo, :datum, ");
        if (udrzba is Cisteni cisteni)
        {
            sql.Append(":typUdrzby, :umytoVMycce, :cistenoOzonem);");
            sqlParams.Add(new OracleParameter("typUdrzby", 'c'));
            sqlParams.Add(new OracleParameter("umytoVMycce", cisteni.UmytoVMycce ? 1 : 0));
            sqlParams.Add(new OracleParameter("cistenoOzonem", cisteni.CistenoOzonem ? 1 : 0));
        }
        else if (udrzba is Oprava oprava)
        {
            sql.Append(":typUdrzby, :popisUkonu, :cena);");
            sqlParams.Add(new OracleParameter("typUdrzby", 'o'));
            sqlParams.Add(new OracleParameter("popisUkonu", oprava.PopisUkonu));
            sqlParams.Add(new OracleParameter("cena", oprava.Cena));
        }
        await DMLPackageCall(sql.ToString(), [.. sqlParams]);
    }

    public async Task DMLUzivateleAsync(Uzivatel uzivatel)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idUzivatel, :jmeno, :heslo, :idRole);";
        OracleParameter[] sqlParams = [ new OracleParameter("idUzivatel", ConvertId(uzivatel.IdUzivatel)),
                                        new OracleParameter("jmeno", uzivatel.Jmeno),
                                        new OracleParameter("heslo", uzivatel.Heslo),
                                        new OracleParameter("idRole", uzivatel.IdRole)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLVozidlaAsync(Vozidlo vozidlo)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idVozidlo,:rokVyroby, :najeteKilometry, :kapacita, :maKlimatizaci, :idGaraz, :idModel);";
        OracleParameter[] sqlParams = [ new OracleParameter("idVozidlo", ConvertId(vozidlo.IdVozidlo)),
                                        new OracleParameter("rokVyroby", vozidlo.RokVyroby),
                                        new OracleParameter("najeteKilometry", vozidlo.NajeteKilometry),
                                        new OracleParameter("kapacita", vozidlo.Kapacita),
                                        new OracleParameter("maKlimatizaci", vozidlo.MaKlimatizaci ? 1 : 0),
                                        new OracleParameter("idGaraz", vozidlo.IdGaraz),
                                        new OracleParameter("idModel", vozidlo.IdModel)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLZastavkyAsync(Zastavka zastavka)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idZastavka, :nazev, :souradniceX, :souradniceY, :idPasmo);";
        OracleParameter[] sqlParams = [ new OracleParameter("idZastavka", ConvertId(zastavka.IdZastavka)),
                                        new OracleParameter("nazev", zastavka.Nazev),
                                        new OracleParameter("souradniceX", zastavka.SouradniceX),
                                        new OracleParameter("souradniceY", zastavka.SouradniceY),
                                        new OracleParameter("idPasmo", zastavka.IdPasmo)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLZaznamy_TrasyAsync(ZaznamTrasy zaznamTrasy)
    {
        string sql = $"{ConvertMethodNameToDML()}(:idZaznam, :idSpoj, :idZastavka, :idVozidlo, :casPrijezdu, :casOdjezdu);";
        OracleParameter[] sqlParams = [ new OracleParameter("idZaznam", ConvertId(zaznamTrasy.IdZaznam)),
                                        new OracleParameter("idSpoj", zaznamTrasy.IdSpoj),
                                        new OracleParameter("idZastavka", zaznamTrasy.IdZastavka),
                                        new OracleParameter("idVozidlo", zaznamTrasy.IdVozidlo),
                                        new OracleParameter("casPrijezdu", zaznamTrasy.CasPrijezdu),
                                        new OracleParameter("casOdjezdu", zaznamTrasy.CasOdjezdu)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLZnackyAsync(Znacka znacka)
    {
        string sql = $"{ConvertMethodNameToDML()}(:ídZnacka, :nazev);";
        OracleParameter[] sqlParams = [ new OracleParameter("ídZnacka", ConvertId(znacka.IdZnacka)),
                                        new OracleParameter("nazev", znacka.Nazev)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task<List<Garaz>?> GetGarazeAsync()
    {
        return await GetDBView<Garaz>(ConvertMethodNameToView());
    }

    public async Task<List<JizniRad>?> GetJizdni_RadyAsync()
    {
        return await GetDBView<JizniRad>(ConvertMethodNameToView());
    }

    public async Task<List<Linka>?> GetLinkyAsync()
    {
        return await GetDBView<Linka>(ConvertMethodNameToView());
    }

    public async Task<List<Model>?> GetModelyAsync()
    {
        return await GetDBView<Model>(ConvertMethodNameToView());
    }

    public async Task<List<Role>?> GetRoleAsync()
    {
        return await GetDBView<Role>(ConvertMethodNameToView());
    }

    public async Task<Role?> GetRoleById(int roleId)
    {
        string sql = @"DECLARE
                     v_role_json CLOB;
                     BEGIN
                     v_role_json := GetRoleById(:p_role_id);
                     :p_result := v_role_json;
                     END;";
        OracleParameter[] sqlParams = [new OracleParameter("p_role_id", OracleDbType.Int32, roleId, ParameterDirection.Input)];
        return await GetObjectFromDB<Role>(sql, sqlParams);
    }

    public async Task<List<Spoj>?> GetSpojeAsync()
    {
        return await GetDBView<Spoj>(ConvertMethodNameToView());
    }

    public async Task<List<TarifniPasmo>?> GetTarifni_PasmaAsync()
    {
        return await GetDBView<TarifniPasmo>(ConvertMethodNameToView());
    }

    public async Task<List<TypVozidla>?> GetTypy_VozidelAsync()
    {
        return await GetDBView<TypVozidla>(ConvertMethodNameToView());
    }

    public async Task<Uzivatel?> GetUzivatelByIdAsync(int id)
    {
        string sql = @"DECLARE
                     v_uzivatel_json CLOB;
                     BEGIN
                     v_uzivatel_json := GetUzivatelById(:p_id_uzivatel);
                     :p_result := v_uzivatel_json;
                     END;";
        OracleParameter[] sqlParams = [new OracleParameter("p_id_uzivatel", OracleDbType.Int32, id, ParameterDirection.Input)];
        return await GetObjectFromDB<Uzivatel>(sql, sqlParams);
    }

    public async Task<Uzivatel?> GetUzivatelByNamePwdAsync(string name, string pwdHash)
    {
        string sql = @"DECLARE
                     v_uzivatel_json CLOB;
                     BEGIN
                     v_uzivatel_json := GetUzivatelByJmenoHash(:p_jmeno_uzivatel,:p_hash_uzivatel);
                     :p_result := v_uzivatel_json;
                     END;";
        OracleParameter[] sqlParams = [ new OracleParameter("p_jmeno_uzivatel", OracleDbType.Varchar2, name, ParameterDirection.Input),
                                        new OracleParameter("p_hash_uzivatel", OracleDbType.Varchar2, pwdHash, ParameterDirection.Input)];
        return await GetObjectFromDB<Uzivatel>(sql, sqlParams);
    }

    public async Task<List<Uzivatel>?> GetUzivateleAsync()
    {
        return await GetDBView<Uzivatel>(ConvertMethodNameToView());
    }

    public async Task<List<Vozidlo>?> GetVozidlaAsync()
    {
        return await GetDBView<Vozidlo>(ConvertMethodNameToView());
    }

    public async Task<Vozidlo?> GetVozidloByIdAsync(int id)
    {
        string sql = @"DECLARE
                     v_vozidlo_json CLOB;
                     BEGIN
                     v_vozidlo_json := GetVozidloById(:p_id_vozidlo);
                     :p_result := v_vozidlo_json;
                     END;";
        OracleParameter[] sqlParams = [new OracleParameter("p_id_vozidlo", OracleDbType.Int32, id, ParameterDirection.Input)];
        return await GetObjectFromDB<Vozidlo>(sql, sqlParams);
    }

    public async Task<Zastavka?> GetZastavkaByIdAsync(int id)
    {
        string sql = @"DECLARE
                     v_zastavka_json CLOB;
                     BEGIN
                     v_zastavka_json := GetZastavkaById(:p_id_zastavka);
                     :p_result := v_zastavka_json;
                     END;";
        OracleParameter[] sqlParams = [new OracleParameter("p_id_zastavka", OracleDbType.Int32, id, ParameterDirection.Input)];
        return await GetObjectFromDB<Zastavka>(sql, sqlParams);
    }

    public async Task<List<Udrzba>?> GetUdrzbyAsync()
    {
        return await GetDBView<Udrzba>(ConvertMethodNameToView());
    }

    public async Task<List<Zastavka>?> GetZastavkyAsync()
    {
        return await GetDBView<Zastavka>(ConvertMethodNameToView());
    }

    public async Task<List<ZaznamTrasy>?> GetZaznamy_TrasyAsync()
    {
        return await GetDBView<ZaznamTrasy>(ConvertMethodNameToView());
    }

    public async Task<List<Znacka>?> GetZnackyAsync()
    {
        return await GetDBView<Znacka>(ConvertMethodNameToView());
    }

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
        modelBuilder.Entity<Udrzba>()
            .HasDiscriminator<char>("TYP_UDRZBY")
            .HasValue<Cisteni>('c')
            .HasValue<Oprava>('o')
            .HasValue<Udrzba>('x');
    }

    private static int? ConvertId(int id) => id == 0 ? null : id;

    private static string ConvertMethodNameToDML([CallerMemberName] string methodName = "") => methodName.Replace("DML", "DML_").Replace("Async", string.Empty).ToUpper();

    private static string ConvertMethodNameToView([CallerMemberName] string methodName = "") => methodName.Replace("Get", string.Empty).Replace("Async", string.Empty).ToLower();

    private async Task DMLPackageCall(string sql, OracleParameter[] sqlParams)
    {
        using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = "BEGIN DML_PROCEDURY." + sql + "END;";
        command.CommandType = CommandType.Text;
        command.Parameters.AddRange(sqlParams);
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    private async Task<List<T>?> GetDBView<T>(string modelName, int? id = null) where T : class
    {
        StringBuilder sql = new(@$"DECLARE
                     v_{modelName}_json CLOB;
                     BEGIN
                     SELECT JSON_ARRAYAGG(JSON_OBJECT(*) RETURNING CLOB) INTO v_{modelName}_json FROM {modelName.ToUpper()}_VIEW");
        if (id != null)
            sql.Append($" WHERE ID_{modelName.ToUpper()} = {id}");
        sql.Append(@$";
                     :p_result := v_{modelName}_json;
                     END;");
        return await GetObjectFromDB<List<T>>(sql.ToString());
    }

    private async Task<T?> GetObjectFromDB<T>(string sql, OracleParameter[]? sqlParams = null) where T : class
    {
        try
        {
            var connection = Database.GetDbConnection();
            var resultParam = new OracleParameter("p_result", OracleDbType.Clob, ParameterDirection.Output);
            string resultJson = string.Empty;

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
            //TODO Deserializace typu údržby
            return JsonConvert.DeserializeObject<T>(resultJson);
        }
        catch (Exception e)
        {
            return null;
        }
    }
}