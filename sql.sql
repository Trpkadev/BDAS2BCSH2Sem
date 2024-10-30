CREATE OR REPLACE PROCEDURE CreateUser (
    p_Jmeno IN VARCHAR2,
    p_Heslo IN VARCHAR2,
    p_IdRole IN NUMBER
) AS
BEGIN
    INSERT INTO Uzivatele (JMENO, HESLO, ID_ROLE)
    VALUES (p_Jmeno, p_Heslo, p_IdRole);
    COMMIT;
END CreateUser;
/



CREATE OR REPLACE PROCEDURE CreateZastavka (
    p_Nazev IN VARCHAR2,
    p_SouradniceX IN NUMBER,
    p_SouradniceY IN NUMBER,
    p_IdPasmo IN NUMBER
) AS
BEGIN
    INSERT INTO Zastavky (NAZEV, SOURADNICE_X, SOURADNICE_Y, ID_PASMO)
    VALUES (p_Nazev, p_SouradniceX, p_SouradniceY, p_IdPasmo);
    COMMIT;
END CreateZastavka;
/


CREATE OR REPLACE PROCEDURE CreateVozidlo (
    p_RokVyroby IN NUMBER,
    p_NajeteKilometry IN NUMBER,
    p_Kapacita IN NUMBER,
    p_MaKlimatizaci IN NUMBER,  -- Boolean is represented as 0 (false) or 1 (true)
    p_IdGaraz IN NUMBER,
    p_IdModel IN NUMBER
) AS
BEGIN
    INSERT INTO Vozidla (ROK_VYROBY, NAJETE_KILOMETRY, KAPACITA, MA_KLIMATIZACI, ID_GARAZ, ID_MODEL)
    VALUES (p_RokVyroby, p_NajeteKilometry, p_Kapacita, p_MaKlimatizaci, p_IdGaraz, p_IdModel);
    COMMIT;
END CreateVozidlo;
/


CREATE OR REPLACE FUNCTION GetVozidloById(p_id_vozidlo IN NUMBER)
RETURN SYS_REFCURSOR
IS
    vozidla_cursor SYS_REFCURSOR;
BEGIN
    OPEN vozidla_cursor FOR
        SELECT *
        FROM VOZIDLA
        WHERE ID_VOZIDLO = p_id_vozidlo;

    RETURN vozidla_cursor;
END;
/


CREATE OR REPLACE FUNCTION GetZastavkaById(p_id_zastavka IN NUMBER)
RETURN SYS_REFCURSOR
IS
    zastavka_cursor SYS_REFCURSOR;
BEGIN
    OPEN zastavka_cursor FOR
        SELECT *
        FROM ZASTAVKY
        WHERE ID_ZASTAVKA = p_id_zastavka;

    RETURN zastavka_cursor;
END;