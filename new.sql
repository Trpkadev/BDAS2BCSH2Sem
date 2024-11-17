CREATE OR REPLACE PROCEDURE CreateUser(
    p_Jmeno IN VARCHAR2,
    p_Heslo IN VARCHAR2
) AS
BEGIN
    INSERT INTO Uzivatele (JMENO, HESLO, ID_ROLE)
    VALUES (p_Jmeno, p_Heslo, 1);
    COMMIT;
END CreateUser;
/


CREATE OR REPLACE PROCEDURE CreateZastavka(
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


CREATE OR REPLACE PROCEDURE CreateVozidlo(
    p_RokVyroby IN NUMBER,
    p_NajeteKilometry IN NUMBER,
    p_Kapacita IN NUMBER,
    p_MaKlimatizaci IN NUMBER, -- Boolean is represented as 0 (false) or 1 (true)
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
    RETURN CLOB IS
    vozidlo_json CLOB;
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
    RETURN CLOB IS
    zastavka_json CLOB;
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
    RETURN CLOB IS
    uzivatel_json CLOB;
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


CREATE TABLE ROLE
(
    ID_ROLE NUMBER PRIMARY KEY,
    NAZEV   VARCHAR2(32) NOT NULL
);
/


CREATE SEQUENCE ROLE_ID_ROLE_SEQ
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;
/


CREATE OR REPLACE TRIGGER ROLE_ID_TRG
    BEFORE INSERT
    ON ROLE
    FOR EACH ROW
    WHEN (NEW.ID_ROLE IS NULL)
BEGIN
    SELECT ROLE_ID_ROLE_SEQ.NEXTVAL
    INTO :NEW.ID_ROLE
    FROM dual;
END;
/


INSERT INTO ROLE (ID_ROLE, NAZEV)
VALUES (0, 'Neprihlaseny');
INSERT INTO ROLE (ID_ROLE, NAZEV)
VALUES (1, 'Prihlaseny');
INSERT INTO ROLE (ID_ROLE, NAZEV)
VALUES (2, 'Udrzbar');
INSERT INTO ROLE (ID_ROLE, NAZEV)
VALUES (3, 'Dispatch');
INSERT INTO ROLE (ID_ROLE, NAZEV)
VALUES (4, 'IT Admin');
INSERT INTO ROLE (ID_ROLE, NAZEV)
VALUES (5, 'Majitel');
/


CREATE TABLE "UZIVATELE"
(
    "ID_UZIVATEL" NUMBER,
    "JMENO"       VARCHAR2(64),
    "HESLO"       VARCHAR2(128),
    "ID_ROLE"     NUMBER
);
CREATE UNIQUE INDEX "UZIVATEL_PK" ON "UZIVATELE" ("ID_UZIVATEL");
CREATE OR REPLACE EDITIONABLE TRIGGER "UZIVATELE_ID_UZIVATEL_TRG"
    BEFORE INSERT
    ON UZIVATELE
    FOR EACH ROW
    WHEN (new.ID_UZIVATEL IS NULL)
BEGIN
    :new.ID_UZIVATEL := UZIVATELE_ID_UZIVATEL_SEQ.nextval;
END;
ALTER TABLE "UZIVATELE"
    ADD CONSTRAINT "UZIVATEL_PK" PRIMARY KEY ("ID_UZIVATEL");
ALTER TABLE "UZIVATELE"
    ADD CONSTRAINT "UZIVATEL_ROLE_FK" FOREIGN KEY ("ID_ROLE") REFERENCES "ROLE" ("ID_ROLE") ENABLE;
ALTER TABLE "UZIVATELE"
    ADD CONSTRAINT "UZIVATEL_JMENO_UQ" UNIQUE ("JMENO")
/


CREATE OR REPLACE FUNCTION GetUzivatelByJmenoHash(p_jmeno_uzivatel VARCHAR2, p_hash_uzivatel VARCHAR2)
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
    WHERE JMENO = p_jmeno_uzivatel
      AND HESLO = p_hash_uzivatel;
    RETURN uzivatel_json;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END;
/


CREATE OR REPLACE FUNCTION GetUzivatele
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


CREATE OR REPLACE FUNCTION GetVozidla
    RETURN CLOB IS
    vozidla_json CLOB;
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


CREATE OR REPLACE FUNCTION GetZastavky
    RETURN CLOB IS
    zastavky_json CLOB;
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

CREATE TABLE LOGY
(
    id_log        NUMBER PRIMARY KEY,
    cas           DATE           NOT NULL,
    typ_zmeny     VARCHAR2(10)   NOT NULL,
    tabulka       VARCHAR2(30)   NOT NULL,
    nove_hodnoty  VARCHAR2(1000) NOT NULL,
    stare_hodnoty VARCHAR2(1000) NOT NULL
);

CREATE SEQUENCE logy_id_log_seq
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

CREATE OR REPLACE TRIGGER logy_id_log_trg
    BEFORE INSERT
    ON logy
    FOR EACH ROW
    WHEN (new.id_log IS NULL)
BEGIN
    :new.id_log := logy_id_log_seq.nextval;
END;
/

CREATE OR REPLACE VIEW VOZIDLA_VIEW AS
SELECT *
FROM VOZIDLA;

CREATE OR REPLACE VIEW GARAZE_VIEW AS
SELECT *
FROM GARAZE;

CREATE OR REPLACE VIEW MODELY_VIEW AS
SELECT *
FROM MODELY;

CREATE OR REPLACE VIEW ZNACKY_VIEW AS
SELECT *
FROM ZNACKY;

CREATE OR REPLACE VIEW TYPY_VOZIDEL_VIEW AS
SELECT *
FROM TYPY_VOZIDEL;

CREATE OR REPLACE VIEW LINKY_VIEW AS
SELECT *
FROM LINKY;

CREATE OR REPLACE VIEW SPOJE_VIEW AS
SELECT *
FROM SPOJE;

CREATE OR REPLACE VIEW JIZDNI_RADY_VIEW AS
SELECT *
FROM JIZDNI_RADY;

CREATE OR REPLACE VIEW ZAZNAMY_TRASY_VIEW AS
SELECT *
FROM ZAZNAMY_TRASY;

CREATE OR REPLACE VIEW ZASTAVKY_VIEW AS
SELECT *
FROM ZASTAVKY;

CREATE OR REPLACE VIEW TARIFNI_PASMA_VIEW AS
SELECT *
FROM TARIFNI_PASMA;

CREATE OR REPLACE VIEW LOGY_VIEW AS
SELECT *
FROM LOGY;


CREATE PACKAGE DML_PROCEDURY AS

    PROCEDURE DML_VOZIDLA(p_id_vozidlo NUMBER, p_rok_vyroby NUMBER, p_najete_kilometry NUMBER, p_kapacita NUMBER,
                          p_ma_klimatizaci NUMBER, p_id_garaz NUMBER, p_id_model NUMBER);
    PROCEDURE DML_GARAZE(p_id_garaz NUMBER, p_nazev VARCHAR2, p_kapacita NUMBER);
    PROCEDURE DML_MODELY(p_id_model NUMBER, p_nazev VARCHAR2, p_je_nizkopodlazni NUMBER, p_id_znacka NUMBER,
                         p_id_typ_vozidla NUMBER);
    PROCEDURE DML_ZNACKY(p_id_znacka NUMBER, p_nazev VARCHAR2);
    PROCEDURE DML_TYPY_VOZIDEL(p_id_typ_vozidla NUMBER, p_nazev VARCHAR2);
    PROCEDURE DML_LINKY(p_id_linka NUMBER, p_cislo NUMBER, p_nazev VARCHAR2, p_id_typ_vozidla NUMBER);
    PROCEDURE DML_SPOJE(p_id_spoj NUMBER, p_jede_ve_vsedni_den NUMBER, p_jede_v_sobotu NUMBER, p_jede_v_nedeli NUMBER,
                        p_garantovane_nizkopodlazni NUMBER, p_id_linka NUMBER);
    PROCEDURE DML_JIZDNI_RADY(p_cas_prijezdu DATE, p_cas_odjezdu DATE, p_id_zastavka NUMBER, p_id_spoj NUMBER);
    PROCEDURE DML_ZAZNAMY_TRASY(p_id_zaznam NUMBER, p_cas_prijezdu DATE, p_cas_odjezdu DATE, p_id_vozidlo NUMBER,
                                p_id_zastavka NUMBER, p_id_spoj NUMBER);
    PROCEDURE DML_ZASTAVKY(p_id_zastavka NUMBER, p_nazev VARCHAR2, p_souradnice_x NUMBER, p_souradnice_y NUMBER,
                           p_id_pasmo NUMBER);
    PROCEDURE DML_TARIFNI_PASMA(p_id_pasmo NUMBER, p_nazev VARCHAR2);

END;



CREATE PACKAGE BODY DML_PROCEDURY AS

    PROCEDURE DML_VOZIDLA(p_id_vozidlo NUMBER, p_rok_vyroby NUMBER, p_najete_kilometry NUMBER, p_kapacita NUMBER,
                          p_ma_klimatizaci NUMBER, p_id_garaz NUMBER, p_id_model NUMBER) IS
    BEGIN
        IF (p_id_vozidlo IS NULL) THEN
            INSERT INTO VOZIDLA
            VALUES (p_id_vozidlo, p_rok_vyroby, p_najete_kilometry, p_kapacita, p_ma_klimatizaci, p_id_garaz,
                    p_id_model);
        ELSE
            UPDATE VOZIDLA
            SET rok_vyroby=p_rok_vyroby,
                najete_kilometry=p_najete_kilometry,
                kapacita=p_kapacita,
                ma_klimatizaci=p_ma_klimatizaci,
                id_garaz=p_id_garaz,
                id_model=p_id_model
            WHERE id_vozidlo = p_id_vozidlo;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_GARAZE(p_id_garaz NUMBER, p_nazev VARCHAR2, p_kapacita NUMBER) IS
    BEGIN
        IF (p_id_garaz IS NULL) THEN
            INSERT INTO GARAZE VALUES (p_id_garaz, p_nazev, p_kapacita);
        ELSE
            UPDATE GARAZE SET nazev=p_nazev, kapacita=p_kapacita WHERE id_garaz = p_id_garaz;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_MODELY(p_id_model NUMBER, p_nazev VARCHAR2, p_je_nizkopodlazni NUMBER, p_id_znacka NUMBER,
                         p_id_typ_vozidla NUMBER) IS
    BEGIN
        IF (p_id_model IS NULL) THEN
            INSERT INTO MODELY VALUES (p_id_model, p_nazev, p_je_nizkopodlazni, p_id_znacka, p_id_typ_vozidla);
        ELSE
            UPDATE MODELY
            SET nazev=p_nazev,
                je_nizkopodlazni=p_je_nizkopodlazni,
                id_znacka=p_id_znacka,
                id_typ_vozidla=p_id_typ_vozidla
            WHERE id_model = p_id_model;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_ZNACKY(p_id_znacka NUMBER, p_nazev VARCHAR2) IS
    BEGIN
        IF (p_id_znacka IS NULL) THEN
            INSERT INTO ZNACKY VALUES (p_id_znacka, p_nazev);
        ELSE
            UPDATE ZNACKY SET nazev=p_nazev WHERE id_znacka = p_id_znacka;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_TYPY_VOZIDEL(p_id_typ_vozidla NUMBER, p_nazev VARCHAR2) IS
    BEGIN
        IF (p_id_typ_vozidla IS NULL) THEN
            INSERT INTO TYPY_VOZIDEL VALUES (p_id_typ_vozidla, p_nazev);
        ELSE
            UPDATE TYPY_VOZIDEL SET nazev=p_nazev WHERE id_typ_vozidla = p_id_typ_vozidla;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_LINKY(p_id_linka NUMBER, p_cislo NUMBER, p_nazev VARCHAR2, p_id_typ_vozidla NUMBER) IS
    BEGIN
        IF (p_id_linka IS NULL) THEN
            INSERT INTO LINKY VALUES (p_id_linka, p_cislo, p_nazev, p_id_typ_vozidla);
        ELSE
            UPDATE LINKY SET cislo=p_cislo, nazev=p_nazev, id_typ_vozidla=p_id_typ_vozidla WHERE id_linka = p_id_linka;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_SPOJE(p_id_spoj NUMBER, p_jede_ve_vsedni_den NUMBER, p_jede_v_sobotu NUMBER, p_jede_v_nedeli NUMBER,
                        p_garantovane_nizkopodlazni NUMBER, p_id_linka NUMBER) IS
    BEGIN
        IF (p_id_spoj IS NULL) THEN
            INSERT INTO SPOJE
            VALUES (p_id_spoj, p_jede_ve_vsedni_den, p_jede_v_sobotu, p_jede_v_nedeli, p_garantovane_nizkopodlazni,
                    p_id_linka);
        ELSE
            UPDATE SPOJE
            SET jede_ve_vsedni_den=p_jede_ve_vsedni_den,
                jede_v_sobotu=p_jede_v_sobotu,
                jede_v_nedeli=p_jede_v_nedeli,
                garantovane_nizkopodlazni=p_garantovane_nizkopodlazni,
                id_linka=p_id_linka
            WHERE id_spoj = p_id_spoj;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_JIZDNI_RADY(p_cas_prijezdu DATE, p_cas_odjezdu DATE, p_id_zastavka NUMBER, p_id_spoj NUMBER) IS
    BEGIN
        IF (p_cas_prijezdu IS NULL) THEN
            INSERT INTO JIZDNI_RADY VALUES (p_cas_prijezdu, p_cas_odjezdu, p_id_zastavka, p_id_spoj);
        ELSE
            UPDATE JIZDNI_RADY
            SET cas_odjezdu=p_cas_odjezdu,
                id_zastavka=p_id_zastavka,
                id_spoj=p_id_spoj
            WHERE cas_prijezdu = p_cas_prijezdu;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_ZAZNAMY_TRASY(p_id_zaznam NUMBER, p_cas_prijezdu DATE, p_cas_odjezdu DATE, p_id_vozidlo NUMBER,
                                p_id_zastavka NUMBER, p_id_spoj NUMBER) IS
    BEGIN
        IF (p_id_zaznam IS NULL) THEN
            INSERT INTO ZAZNAMY_TRASY
            VALUES (p_id_zaznam, p_cas_prijezdu, p_cas_odjezdu, p_id_vozidlo, p_id_zastavka, p_id_spoj);
        ELSE
            UPDATE ZAZNAMY_TRASY
            SET cas_prijezdu=p_cas_prijezdu,
                cas_odjezdu=p_cas_odjezdu,
                id_vozidlo=p_id_vozidlo,
                id_zastavka=p_id_zastavka,
                id_spoj=p_id_spoj
            WHERE id_zaznam = p_id_zaznam;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_ZASTAVKY(p_id_zastavka NUMBER, p_nazev VARCHAR2, p_souradnice_x NUMBER, p_souradnice_y NUMBER,
                           p_id_pasmo NUMBER) IS
    BEGIN
        IF (p_id_zastavka IS NULL) THEN
            INSERT INTO ZASTAVKY VALUES (p_id_zastavka, p_nazev, p_souradnice_x, p_souradnice_y, p_id_pasmo);
        ELSE
            UPDATE ZASTAVKY
            SET nazev=p_nazev,
                souradnice_x=p_souradnice_x,
                souradnice_y=p_souradnice_y,
                id_pasmo=p_id_pasmo
            WHERE id_zastavka = p_id_zastavka;
        END IF;
        COMMIT;
    END;

    PROCEDURE DML_TARIFNI_PASMA(p_id_pasmo NUMBER, p_nazev VARCHAR2) IS
    BEGIN
        IF (p_id_pasmo IS NULL) THEN
            INSERT INTO TARIFNI_PASMA VALUES (p_id_pasmo, p_nazev);
        ELSE
            UPDATE TARIFNI_PASMA SET nazev=p_nazev WHERE id_pasmo = p_id_pasmo;
        END IF;
        COMMIT;
    END;

END;


CREATE OR REPLACE TRIGGER TRG_LOG_VOZIDLA
    BEFORE INSERT OR UPDATE OR DELETE
    ON VOZIDLA
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'VOZIDLA',
            :NEW.id_vozidlo || '; ' || :NEW.rok_vyroby || '; ' || :NEW.najete_kilometry || '; ' || :NEW.kapacita ||
            '; ' || :NEW.ma_klimatizaci || '; ' || :NEW.id_garaz || '; ' || :NEW.id_model,
            :OLD.id_vozidlo || '; ' || :OLD.rok_vyroby || '; ' || :OLD.najete_kilometry || '; ' || :OLD.kapacita ||
            '; ' || :OLD.ma_klimatizaci || '; ' || :OLD.id_garaz || '; ' || :OLD.id_model);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_GARAZE
    BEFORE INSERT OR UPDATE OR DELETE
    ON GARAZE
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'GARAZE', :NEW.id_garaz || '; ' || :NEW.nazev || '; ' || :NEW.kapacita,
            :OLD.id_garaz || '; ' || :OLD.nazev || '; ' || :OLD.kapacita);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_MODELY
    BEFORE INSERT OR UPDATE OR DELETE
    ON MODELY
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'MODELY',
            :NEW.id_model || '; ' || :NEW.nazev || '; ' || :NEW.je_nizkopodlazni || '; ' || :NEW.id_znacka || '; ' ||
            :NEW.id_typ_vozidla,
            :OLD.id_model || '; ' || :OLD.nazev || '; ' || :OLD.je_nizkopodlazni || '; ' || :OLD.id_znacka || '; ' ||
            :OLD.id_typ_vozidla);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_ZNACKY
    BEFORE INSERT OR UPDATE OR DELETE
    ON ZNACKY
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'ZNACKY', :NEW.id_znacka || '; ' || :NEW.nazev, :OLD.id_znacka || '; ' || :OLD.nazev);
END ;

CREATE OR REPLACE TRIGGER TRG_LOG_TYPY_VOZIDEL
    BEFORE INSERT OR UPDATE OR DELETE
    ON TYPY_VOZIDEL
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'TYPY_VOZIDEL', :NEW.id_typ_vozidla || '; ' || :NEW.nazev,
            :OLD.id_typ_vozidla || '; ' || :OLD.nazev);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_LINKY
    BEFORE INSERT OR UPDATE OR DELETE
    ON LINKY
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'LINKY',
            :NEW.id_linka || '; ' || :NEW.cislo || '; ' || :NEW.nazev || '; ' || :NEW.id_typ_vozidla,
            :OLD.id_linka || '; ' || :OLD.cislo || '; ' || :OLD.nazev || '; ' || :OLD.id_typ_vozidla);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_SPOJE
    BEFORE INSERT OR UPDATE OR DELETE
    ON SPOJE
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'SPOJE',
            :NEW.id_spoj || '; ' || :NEW.jede_ve_vsedni_den || '; ' || :NEW.jede_v_sobotu || '; ' ||
            :NEW.jede_v_nedeli || '; ' || :NEW.garantovane_nizkopodlazni || '; ' || :NEW.id_linka,
            :OLD.id_spoj || '; ' || :OLD.jede_ve_vsedni_den || '; ' || :OLD.jede_v_sobotu || '; ' ||
            :OLD.jede_v_nedeli || '; ' || :OLD.garantovane_nizkopodlazni || '; ' || :OLD.id_linka);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_JIZDNI_RADY
    BEFORE INSERT OR UPDATE OR DELETE
    ON JIZDNI_RADY
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'JIZDNI_RADY',
            :NEW.cas_prijezdu || '; ' || :NEW.cas_odjezdu || '; ' || :NEW.id_zastavka || '; ' || :NEW.id_spoj,
            :OLD.cas_prijezdu || '; ' || :OLD.cas_odjezdu || '; ' || :OLD.id_zastavka || '; ' || :OLD.id_spoj);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_ZAZNAMY_TRASY
    BEFORE INSERT OR UPDATE OR DELETE
    ON ZAZNAMY_TRASY
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'ZAZNAMY_TRASY',
            :NEW.id_zaznam || '; ' || :NEW.cas_prijezdu || '; ' || :NEW.cas_odjezdu || '; ' || :NEW.id_vozidlo ||
            '; ' || :NEW.id_zastavka || '; ' || :NEW.id_spoj,
            :OLD.id_zaznam || '; ' || :OLD.cas_prijezdu || '; ' || :OLD.cas_odjezdu || '; ' || :OLD.id_vozidlo ||
            '; ' || :OLD.id_zastavka || '; ' || :OLD.id_spoj);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_ZASTAVKY
    BEFORE INSERT OR UPDATE OR DELETE
    ON ZASTAVKY
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'ZASTAVKY',
            :NEW.id_zastavka || '; ' || :NEW.nazev || '; ' || :NEW.souradnice_x || '; ' || :NEW.souradnice_y || '; ' ||
            :NEW.id_pasmo,
            :OLD.id_zastavka || '; ' || :OLD.nazev || '; ' || :OLD.souradnice_x || '; ' || :OLD.souradnice_y || '; ' ||
            :OLD.id_pasmo);
END;
/

CREATE OR REPLACE TRIGGER TRG_LOG_TARIFNI_PASMA
    BEFORE INSERT OR UPDATE OR DELETE
    ON TARIFNI_PASMA
    FOR EACH ROW
DECLARE
    v_typ VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_typ := 'INSERT';
    ELSIF UPDATING THEN
        v_typ := 'UPDATE';
    ELSE
        v_typ := 'DELETE';
    END IF;
    INSERT INTO LOGY
    VALUES (NULL, SYSDATE, v_typ, 'TARIFNI_PASMA', :NEW.id_pasmo || '; ' || :NEW.nazev || '; ',
            :OLD.id_pasmo || '; ' || :OLD.nazev);
END ;
/


CREATE OR REPLACE VIEW PRUMERNE_ZPOZDENI AS
SELECT MIN(L.CISLO) linka, ROUND(AVG(ZT.CAS_ODJEZDU - JR.CAS_ODJEZDU), 2) zpozdeni
FROM JIZDNI_RADY JR
         JOIN ZAZNAMY_TRASY ZT ON JR.ID_ZASTAVKA = ZT.ID_ZASTAVKA AND JR.ID_SPOJ = ZT.ID_SPOJ
         JOIN SPOJE S ON S.ID_SPOJ = JR.ID_SPOJ
         JOIN LINKY L ON L.ID_LINKA = S.ID_LINKA
GROUP BY L.ID_LINKA;

CREATE OR REPLACE VIEW NAKLADY_VOZIDLA AS
SELECT MIN(Z.NAZEV) znacka, MIN(M.NAZEV) model, ROUND(AVG(U.CENA / V.NAJETE_KILOMETRY), 2) naklady
FROM UDRZBY U
         JOIN VOZIDLA V ON V.ID_VOZIDLO = U.ID_VOZIDLO
         JOIN MODELY M ON M.ID_MODEL = V.ID_MODEL
         JOIN ZNACKY Z ON Z.ID_ZNACKA = M.ID_ZNACKA
GROUP BY M.ID_MODEL;

CREATE OR REPLACE VIEW DB_OBJEKTY AS
SELECT OBJECT_NAME nazev, OBJECT_TYPE typ, CREATED vytvoreno, LAST_ANALYZED posledni_pristup
FROM USER_OBJECTS
         LEFT JOIN USER_TABLES ON OBJECT_NAME = TABLE_NAME
ORDER BY OBJECT_NAME;