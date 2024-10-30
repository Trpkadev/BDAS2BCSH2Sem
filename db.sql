create sequence GARAZE_ID_GARAZ_SEQ
    order
    nocache
/

create sequence LINKY_ID_LINKA_SEQ
    order
    nocache
/

create sequence MODELY_ID_MODEL_SEQ
    order
    nocache
/

create sequence SPOJE_ID_SPOJ_SEQ
    order
    nocache
/

create sequence TARIFNI_PASMA_ID_PASMO_SEQ
    order
    nocache
/

create sequence TYPY_VOZIDEL_ID_TYP_VOZIDLA
    order
    nocache
/

create sequence UDRZBY_ID_UDRZBA_SEQ
    order
    nocache
/

create sequence VOZIDLA_ID_VOZIDLO_SEQ
    order
    nocache
/

create sequence ZASTAVKY_ID_ZASTAVKA_SEQ
    order
    nocache
/

create sequence ZAZNAMY_TRASY_ID_ZAZNAM_SEQ
    order
    nocache
/

create sequence ZNACKY_ID_ZNACKA_SEQ
    order
    nocache
/

create sequence UZIVATELE_ID_UZIVATEL_SEQ
    order
    nocache
/

create sequence ROLE_ID_ROLE_SEQ
    order
    nocache
/

create table GARAZE
(
    ID_GARAZ NUMBER       not null
        constraint GARAZ_PK
            primary key,
    NAZEV    VARCHAR2(30) not null
        constraint GARAZ_NAZEV_UK
            unique,
    KAPACITA NUMBER(3)    not null
)
/

create trigger GARAZE_ID_GARAZ_TRG
    before insert
    on GARAZE
    for each row
    when (new.id_garaz IS NULL)
BEGIN
    :new.id_garaz := garaze_id_garaz_seq.nextval;
END;
/

create table TARIFNI_PASMA
(
    ID_PASMO NUMBER       not null
        constraint TARIFNI_PASMO_PK
            primary key,
    NAZEV    VARCHAR2(30) not null
)
/

create trigger TARIFNI_PASMA_ID_PASMO_TRG
    before insert
    on TARIFNI_PASMA
    for each row
    when (new.id_pasmo IS NULL)
BEGIN
    :new.id_pasmo := tarifni_pasma_id_pasmo_seq.nextval;
END;
/

create table TYPY_VOZIDEL
(
    ID_TYP_VOZIDLA NUMBER       not null
        constraint TYP_VOZIDLA_PK
            primary key,
    NAZEV          VARCHAR2(30) not null
        constraint TYP_VOZIDLA_NAZEV_UK
            unique
)
/

create table LINKY
(
    ID_LINKA       NUMBER        not null
        constraint LINKA_PK
            primary key,
    CISLO          NUMBER(3)     not null
        constraint LINKY_CISLO_CK
            check (cislo > 0),
    NAZEV          VARCHAR2(255) not null
        constraint LINKA_NAZEV_UK
            unique,
    ID_TYP_VOZIDLA NUMBER        not null
        constraint LINKA_TYP_VOZIDLA_FK
            references TYPY_VOZIDEL
)
/

create trigger LINKY_ID_LINKA_TRG
    before insert
    on LINKY
    for each row
    when (new.id_linka IS NULL)
BEGIN
    :new.id_linka := linky_id_linka_seq.nextval;
END;
/

create table SPOJE
(
    ID_SPOJ                   NUMBER    not null
        constraint SPOJ_PK
            primary key,
    JEDE_VE_VSEDNI_DEN        NUMBER(1) not null,
    JEDE_V_SOBOTU             NUMBER(1) not null,
    JEDE_V_NEDELI             NUMBER(1) not null,
    GARANTOVANA_NIZKOPODLAZNI NUMBER(1) not null,
    ID_LINKA                  NUMBER    not null
        constraint SPOJ_LINKA_FK
            references LINKY
)
/

create trigger FKNTM_SPOJE
    before update of ID_LINKA
    on SPOJE
BEGIN
    raise_application_error(-20225, 'Non Transferable FK constraint  on table SPOJE is violated');
END;
/

create trigger SPOJE_ID_SPOJ_TRG
    before insert
    on SPOJE
    for each row
    when (new.id_spoj IS NULL)
BEGIN
    :new.id_spoj := spoje_id_spoj_seq.nextval;
END;
/

create trigger TYPY_VOZIDEL_ID_TYP_VOZIDLA
    before insert
    on TYPY_VOZIDEL
    for each row
    when (new.id_typ_vozidla IS NULL)
BEGIN
    :new.id_typ_vozidla := typy_vozidel_id_typ_vozidla.nextval;
END;
/

create table ZASTAVKY
(
    ID_ZASTAVKA  NUMBER         not null
        constraint ZASTAVKA_PK
            primary key,
    NAZEV        VARCHAR2(60)   not null,
    SOURADNICE_X NUMBER(15, 12) not null,
    SOURADNICE_Y NUMBER(15, 12) not null,
    ID_PASMO     NUMBER         not null
        constraint ZASTAVKA_TARIFNI_PASMO_FK
            references TARIFNI_PASMA,
    constraint ZASTAVKA_SOURADNICE_UK
        unique (SOURADNICE_X, SOURADNICE_Y)
)
/

create table JIZDNI_RADY
(
    CAS_PRIJEZDU DATE,
    CAS_ODJEZDU  DATE   not null,
    ID_ZASTAVKA  NUMBER not null
        constraint JIZDNI_RAD_ZASTAVKA_FK
            references ZASTAVKY,
    ID_SPOJ      NUMBER not null
        constraint JIZDNI_RAD_SPOJ_FK
            references SPOJE,
    constraint JIZDNI_RAD_PK
        primary key (ID_ZASTAVKA, ID_SPOJ),
    constraint JR_CASY_CK
        check (cas_prijezdu < cas_odjezdu)
)
/

create trigger ZASTAVKY_ID_ZASTAVKA_TRG
    before insert
    on ZASTAVKY
    for each row
    when (new.id_zastavka IS NULL)
BEGIN
    :new.id_zastavka := zastavky_id_zastavka_seq.nextval;
END;
/

create table ZNACKY
(
    ID_ZNACKA NUMBER       not null
        constraint ZNACKA_PK
            primary key,
    NAZEV     VARCHAR2(30) not null
        constraint ZNACKA_NAZEV_UK
            unique
)
/

create table MODELY
(
    ID_MODEL         NUMBER       not null
        constraint MODEL_PK
            primary key,
    NAZEV            VARCHAR2(30) not null,
    JE_NIZKOPODLAZNI NUMBER(1)    not null,
    ID_ZNACKA        NUMBER       not null
        constraint MODEL_ZNACKA_FK
            references ZNACKY,
    ID_TYP_VOZIDLA   NUMBER       not null
        constraint MODEL_TYP_VOZDILA_FK
            references TYPY_VOZIDEL
)
/

create trigger MODELY_ID_MODEL_TRG
    before insert
    on MODELY
    for each row
    when (new.id_model IS NULL)
BEGIN
    :new.id_model := modely_id_model_seq.nextval;
END;
/

create table VOZIDLA
(
    ID_VOZIDLO       NUMBER    not null
        constraint VOZIDLO_PK
            primary key,
    ROK_VYROBY       NUMBER(4) not null,
    NAJETE_KILOMETRY NUMBER(8) not null,
    KAPACITA         NUMBER(3) not null,
    MA_KLIMATIZACI   NUMBER(1) not null,
    ID_GARAZ         NUMBER    not null
        constraint VOZIDLO_GARAZ_FK
            references GARAZE,
    ID_MODEL         NUMBER    not null
        constraint VOZIDLO_MODEL_FK
            references MODELY
)
/

create table UDRZBY
(
    ID_UDRZBA      NUMBER not null
        constraint UDRZBA_PK
            primary key,
    DATUM          DATE   not null,
    ID_VOZIDLO     NUMBER not null
        constraint UDRZBA_VOZIDLO_FK
            references VOZIDLA,
    POPIS_UKONU    VARCHAR2(255),
    CENA           NUMBER(10, 2),
    UMYTO_V_MYCCE  NUMBER(1),
    CISTENO_OZONEM NUMBER(1),
    TYP_UDRZBY     CHAR   not null
        constraint UDRZBA_TYP_CK
            check (typ_udrzby IN ('o', 'c'))
)
/

create trigger UDRZBY_ID_UDRZBA_TRG
    before insert
    on UDRZBY
    for each row
    when (new.id_udrzba IS NULL)
BEGIN
    :new.id_udrzba := udrzby_id_udrzba_seq.nextval;
END;
/

create trigger VOZIDLA_ID_VOZIDLO_TRG
    before insert
    on VOZIDLA
    for each row
    when (new.id_vozidlo IS NULL)
BEGIN
    :new.id_vozidlo := vozidla_id_vozidlo_seq.nextval;
END;
/

create table ZAZNAMY_TRASY
(
    ID_ZAZNAM    NUMBER not null
        constraint ZAZNAM_TRASY_PK
            primary key,
    CAS_PRIJEZDU DATE   not null,
    CAS_ODJEZDU  DATE   not null,
    ID_VOZIDLO   NUMBER not null
        constraint ZAZNAM_TRASY_VOZIDLO_FK
            references VOZIDLA,
    ID_ZASTAVKA  NUMBER not null,
    ID_SPOJ      NUMBER not null,
    constraint ZAZNAM_TRASY_JIZDNI_RAD_FK
        foreign key (ID_ZASTAVKA, ID_SPOJ) references JIZDNI_RADY
)
/

create trigger ZAZNAMY_TRASY_ID_ZAZNAM_TRG
    before insert
    on ZAZNAMY_TRASY
    for each row
    when (new.id_zaznam IS NULL)
BEGIN
    :new.id_zaznam := zaznamy_trasy_id_zaznam_seq.nextval;
END;
/

create trigger ZNACKY_ID_ZNACKA_TRG
    before insert
    on ZNACKY
    for each row
    when (new.id_znacka IS NULL)
BEGIN
    :new.id_znacka := znacky_id_znacka_seq.nextval;
END;
/

create table ROLE
(
    ID_ROLE NUMBER       not null
        constraint ROLE_PK
            primary key,
    NAZEV   VARCHAR2(32) not null
)
/

create trigger ROLE_ID_ROLE_TRG
    before insert
    on ROLE
    for each row
    when (new.id_role IS NULL)
BEGIN
    :new.id_role := role_id_role_seq.nextval;
END;
/

create table UZIVATELE
(
    ID_UZIVATEL       NUMBER        not null
        constraint UZIVATEL_PK
            primary key,
    JMENO             VARCHAR2(64)  not null,
    UZIVATELSKE_JMENO VARCHAR2(32)  not null,
    HESLO             VARCHAR2(128) not null,
    ID_ROLE           NUMBER        not null
        constraint UZIVATEL_ROLE_FK
            references ROLE
)
/

create trigger UZIVATELE_ID_UZIVATEL_TRG
    before insert
    on UZIVATELE
    for each row
    when (new.id_uzivatel IS NULL)
BEGIN
    :new.id_uzivatel := uzivatele_id_uzivatel_seq.nextval;
END;
/

create view LINKY_ZKRACENE as
SELECT l.cislo, SUBSTR(l.nazev, 1, INSTR(l.nazev, '-') - 1) || '-' ||
  SUBSTR(l.nazev, INSTR(l.nazev, '-', -1) + 1) zkraceNazev
FROM LINKY l
ORDER BY l.cislo
/
