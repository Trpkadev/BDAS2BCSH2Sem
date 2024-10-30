﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("ZNACKY")]
public class Znacka
{
    [Key]
    [JsonRequired]
    [Column("ID_ZNACKA")]
    public int IdZnacka { get; set; }

    [JsonRequired]
    [Column("NAZEV")]
    public string Nazev { get; set; } = string.Empty;
}