using Integracao.Med.Despesas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Services.Util
{
    public static class Util
    {

        #region OLD
        //public async static Task<bool> VerificaSejaExiste(string id, IHanaAdapter hana, string tabela,string campo, string database)
        //{
        //    try
        //    {
        //        string sQuery = String.Format(@"SELECT COUNT(*) FROM ""{0}"".""{2}"" WHERE ""{3}"" = '{1}'", database, id, tabela,campo);

        //        var ret = hana.ExecuteSinc(sQuery);

        //        if ((long)ret == 0)
        //            return false;
        //        else
        //            return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public async static Task<bool> VerificaSejaExistePed(string id, IHanaAdapter hana, string tabela, string campo, string database,string docEntry,string linhaPed)
        //{
        //    try
        //    {
        //        string sQuery = String.Format(@"SELECT COUNT(*) FROM ""{0}"".""{2}"" WHERE ""{3}"" = '{1}' AND ""U_solicitacao_item_id"" = '{4}' AND ""U_LinhaPed"" = '{5}'", database, id, tabela, campo,docEntry,linhaPed);

        //        var ret = hana.ExecuteSinc(sQuery);

        //        if ((long)ret == 0)
        //            return false;
        //        else
        //            return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public async static Task<bool> VerificaSejaExisteMovTwoFields(string id,string id2, IHanaAdapter hana, string tabela, string campo,string campo2, string database,string tipo)
        //{
        //    try
        //    {
        //        string sQuery = String.Format(@"SELECT COUNT(*) FROM ""{0}"".""{2}"" WHERE ""{3}"" = '{1}' AND ""{4}"" = '{5}' AND ""U_tipo"" = {6}", database, id, tabela, campo,campo2,id2,tipo);

        //        var ret = hana.ExecuteSinc(sQuery);

        //        if ((long)ret == 0)
        //            return false;
        //        else
        //            return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public async static Task<int> AtualizaFlag(string id,IHanaAdapter hana, string tabela, string campo1,string campo2, string database,string valor)
        //{

        //    try
        //    {
        //        //update flag
        //        var query = String.Format(@"UPDATE ""{0}"".""{2}"" SET ""{4}"" = '{5}' WHERE  ""{3}"" = '{1}'",database, id,tabela,campo1,campo2,valor);
        //        var ret =  await hana.Execute(query);

        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }


        //}

        //public async static Task<int> AtualizaFlagMov(string id, IHanaAdapter hana, string tabela, string campo1, string campo2,string campo3, string database, string valor,string docEntry)
        //{

        //    try
        //    {
        //        //update flag
        //        var query = String.Format(@"UPDATE ""{0}"".""{2}"" SET ""{4}"" = '{5}' WHERE  ""{3}"" = '{1}' AND ""{6}"" = '{7}'", database, id, tabela, campo1, campo2, valor,campo3,docEntry);
        //        var ret = await hana.Execute(query);

        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }


        //}

        //public async static Task<int> AtualizaFlagItens(string id, IHanaAdapter hana, string tabela, string campo1, string campo2, string campo3, string database, string valor, string docEntry)
        //{

        //    try
        //    {
        //        //update flag
        //        var query = String.Format(@"UPDATE ""{0}"".""{2}"" SET ""{4}"" = '{5}' WHERE  ""{3}"" = '{1}' ", database, id, tabela, campo1, campo2, valor);
        //        var ret = await hana.Execute(query);

        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }


        //}
        #endregion

    }
}
