﻿using System;
using System.Collections.Generic;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Models
{
    public partial class Usuario:IUsuario
    {
        public Usuario()
        {
            ModsOficinas = new HashSet<ModsOficina>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Usuario1 { get; set; }
        public string Contraseña { get; set; }
        public string Opciones { get; set; }
        public string Oficinas { get; set; }
        public bool? Administrador { get; set; }
        public bool? Inactivo { get; set; }
        public bool? CfgOfi { get; set; }
        public bool? CfgOpc { get; set; }
        public DateTime? UltimaModif { get; set; }

//*** Implementaciones de la interfas
        private IEnumerable<IEnlace> _enlaces;
        public virtual ICollection<ModsOficina> ModsOficinas { get; set; }
string IUsuario.Id { get => this.Id.ToString(); }
        string IUsuario.Usuario { get => this.Usuario1;}
        bool IUsuario.Administrador { get => this.Administrador == true; }
        IEnumerable<IEnlace> IUsuario.Enlaces { get => _enlaces; }

        public void SetEnlaces(IEnumerable<IEnlace> enlaces){
            _enlaces = enlaces;
        }
        public string GetCadEnlaces()
        {
            var stringBuilder = new System.Text.StringBuilder();
            foreach( var enlace in _enlaces){
                stringBuilder.Append($"{enlace.Id};");
            }
            return stringBuilder.ToString();
        }
    }
}
