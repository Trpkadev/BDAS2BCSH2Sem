CREATE OR REPLACE PROCEDURE CreateUser (
    p_Jmeno IN VARCHAR2,
    p_Heslo IN VARCHAR2
) AS
BEGIN
    INSERT INTO Uzivatele (JMENO, HESLO, ID_ROLE)
    VALUES (p_Jmeno, p_Heslo, 1);
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
RETURN CLOB IS vozidlo_json CLOB;
BEGIN
    SELECT JSON_OBJECT(
               'IdVozidlo' VALUE ID_VOZIDLO,
               'RokVyroby' VALUE ROK_VYROBY,
               'NajeteKilometry' VALUE NAJETE_KILOMETRY,
               'Kapacita' VALUE KAPACITA,
               'MaKlimatizaci' VALUE MA_KLIMATIZACI,
               'IdGaraz' VALUE ID_GARAZ,
               'IdModel' VALUE ID_MODEL)
    INTO vozidlo_json
    FROM VOZIDLA
    WHERE ID_VOZIDLO = p_id_vozidlo
    FETCH FIRST 1 ROW ONLY;
    RETURN vozidlo_json;
    
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END;
/


CREATE OR REPLACE FUNCTION GetZastavkaById(p_id_zastavka IN NUMBER)
RETURN CLOB IS zastavka_json CLOB;
BEGIN
    SELECT JSON_OBJECT(
               'IdZastavka' VALUE ID_ZASTAVKA,
               'Nazev' VALUE NAZEV,
               'SouradniceX' VALUE SOURADNICE_X,
               'SouradniceY' VALUE SOURADNICE_Y,
               'IdPasmo' VALUE ID_PASMO)
    INTO zastavka_json
    FROM ZASTAVKY
    WHERE ID_ZASTAVKA = p_id_zastavka
    FETCH FIRST 1 ROW ONLY;
    RETURN zastavka_json;
    
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END;
/


CREATE OR REPLACE FUNCTION GetUzivatelById(p_id_uzivatel IN NUMBER)
RETURN CLOB IS uzivatel_json CLOB;
BEGIN
    SELECT JSON_OBJECT(
               'IdUzivatel' VALUE ID_UZIVATEL,
               'Jmeno' VALUE JMENO,
               'Heslo' VALUE HESLO,
               'IdRole' VALUE ID_ROLE)
    INTO uzivatel_json
    FROM UZIVATELE
    WHERE ID_UZIVATEL = p_id_uzivatel
    FETCH FIRST 1 ROW ONLY;
    RETURN uzivatel_json;
    
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END;
/


CREATE TABLE ROLE (
    ID_ROLE NUMBER PRIMARY KEY,
    NAZEV VARCHAR2(32) NOT NULL
);
/


CREATE SEQUENCE ROLE_ID_ROLE_SEQ
START WITH 1
INCREMENT BY 1
NOCACHE
NOCYCLE;
/


create or replace TRIGGER ROLE_ID_TRG
BEFORE INSERT ON ROLE
FOR EACH ROW
WHEN (NEW.ID_ROLE IS NULL)
BEGIN
    SELECT ROLE_ID_ROLE_SEQ.NEXTVAL
    INTO :NEW.ID_ROLE
    FROM dual;
END;
/


insert into ROLE (ID_ROLE,NAZEV) VALUES (0,'Neprihlaseny');
insert into ROLE (ID_ROLE,NAZEV) VALUES (1,'Prihlaseny');
insert into ROLE (ID_ROLE,NAZEV) VALUES (2,'Udrzbar');
insert into ROLE (ID_ROLE,NAZEV) VALUES (3,'Dispatch');
insert into ROLE (ID_ROLE,NAZEV) VALUES (4,'IT Admin');
insert into ROLE (ID_ROLE,NAZEV) VALUES (5,'Majitel');
/


CREATE TABLE "UZIVATELE" 
   ("ID_UZIVATEL" NUMBER, 
	"JMENO" VARCHAR2(64), 
	"HESLO" VARCHAR2(128), 
	"ID_ROLE" NUMBER);
CREATE UNIQUE INDEX "UZIVATEL_PK" ON "UZIVATELE" ("ID_UZIVATEL");
CREATE OR REPLACE EDITIONABLE TRIGGER "UZIVATELE_ID_UZIVATEL_TRG" 
    before insert
    on UZIVATELE
    for each row
     WHEN (new.ID_UZIVATEL IS NULL) BEGIN
    :new.ID_UZIVATEL := UZIVATELE_ID_UZIVATEL_SEQ.nextval;
END;
  ALTER TABLE "UZIVATELE" ADD CONSTRAINT "UZIVATEL_PK" PRIMARY KEY ("ID_UZIVATEL");
  ALTER TABLE "UZIVATELE" ADD CONSTRAINT "UZIVATEL_ROLE_FK" FOREIGN KEY ("ID_ROLE") REFERENCES "ROLE" ("ID_ROLE") ENABLE;
  ALTER TABLE "UZIVATELE" ADD CONSTRAINT "UZIVATEL_JMENO_UQ" UNIQUE ("JMENO") 
  /


  CREATE OR REPLACE
FUNCTION GetUzivatelByJmenoHash(p_jmeno_uzivatel VARCHAR2,p_hash_uzivatel VARCHAR2)
RETURN CLOB IS
    uzivatel_json CLOB;
BEGIN
    SELECT JSON_OBJECT(
        'IdUzivatel' VALUE ID_UZIVATEL,
        'Jmeno' VALUE JMENO,
        'Heslo' VALUE HESLO,
        'IdRole' VALUE UZIVATELE.ID_ROLE,
        'RoleNazev' VALUE ROLE.NAZEV)           
    INTO uzivatel_json
    FROM UZIVATELE
    JOIN ROLE ON UZIVATELE.ID_ROLE = ROLE.ID_ROLE
    WHERE JMENO = p_jmeno_uzivatel AND HESLO = p_hash_uzivatel;
    RETURN uzivatel_json;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END;
/


CREATE OR REPLACE
FUNCTION GetUzivatele
RETURN CLOB IS
    uzivatele_json CLOB;
BEGIN
    SELECT JSON_ARRAYAGG(
               JSON_OBJECT(
                   'IdUzivatel' VALUE ID_UZIVATEL,
                   'Jmeno' VALUE JMENO,
                   'Heslo' VALUE HESLO,
                   'IdRole' VALUE UZIVATELE.ID_ROLE,
                   'RoleNazev' VALUE ROLE.NAZEV) RETURNS CLOB
           )
    INTO uzivatele_json
    FROM UZIVATELE
    JOIN ROLE ON UZIVATELE.ID_ROLE = ROLE.ID_ROLE;

    RETURN uzivatele_json;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END;
/


CREATE OR REPLACE
FUNCTION GetVozidla
RETURN CLOB IS vozidla_json CLOB;
BEGIN
    SELECT JSON_ARRAYAGG(
               JSON_OBJECT(
               'IdVozidlo' VALUE ID_VOZIDLO,
               'RokVyroby' VALUE ROK_VYROBY,
               'NajeteKilometry' VALUE NAJETE_KILOMETRY,
               'Kapacita' VALUE VOZIDLA.KAPACITA,
               'MaKlimatizaci' VALUE MA_KLIMATIZACI,
               'IdGaraz' VALUE VOZIDLA.ID_GARAZ,
               'IdModel' VALUE VOZIDLA.ID_MODEL,
               'GarazNazev' VALUE GARAZE.NAZEV,
               'ModelNazev' VALUE MODELY.NAZEV) RETURNS CLOB
               )
    INTO vozidla_json
    FROM VOZIDLA
    JOIN GARAZE ON VOZIDLA.ID_GARAZ = GARAZE.ID_GARAZ
    JOIN MODELY ON VOZIDLA.ID_MODEL = MODELY.ID_MODEL;
    RETURN vozidla_json;
    
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END;
/


CREATE OR REPLACE
FUNCTION GetZastavky
RETURN CLOB IS zastavky_json CLOB;
BEGIN
    SELECT JSON_ARRAYAGG(
               JSON_OBJECT(
               'IdZastavka' VALUE ID_ZASTAVKA,
               'Nazev' VALUE ZASTAVKY.NAZEV,
               'SouradniceX' VALUE SOURADNICE_X,
               'SouradniceY' VALUE SOURADNICE_Y,
               'IdPasmo' VALUE ZASTAVKY.ID_PASMO,
               'PasmoNazev' VALUE TARIFNI_PASMA.NAZEV) RETURNS CLOB
               )
    INTO zastavky_json
    FROM ZASTAVKY
    JOIN TARIFNI_PASMA ON ZASTAVKY.ID_PASMO = TARIFNI_PASMA.ID_PASMO;
    RETURN zastavky_json;
    
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END;