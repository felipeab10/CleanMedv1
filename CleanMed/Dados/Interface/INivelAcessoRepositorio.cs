using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface INivelAcessoRepositorio:IRepositorioGenerico<NiveisAcesso>
    {
        Task<bool> NivelAcessoExiste(string NivelAcesso);
    }
}
