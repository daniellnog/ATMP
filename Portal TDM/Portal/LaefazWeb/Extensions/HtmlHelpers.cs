using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace TDMWeb.Extensions
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString InputFile(this HtmlHelper helper, string name, string label)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class='form-group custom-file-upload'>");
            builder.Append(String.Format("    <label>{0}</label>", label));
            builder.Append("    <div class='input-group'>");
            builder.Append(String.Format("      <input style='visibility: hidden; position: absolute; top: -5000px; left: -5000px;' type='file' multiple accept='.xlsb' id = '{0}' name='{0}'>", name));
            builder.Append(String.Format("      <input type='text' class='form-control input-file-filename' name='{0}' placeholder='Nenhum arquivo selecionado...' readonly='readonly'>", name));
            builder.Append("        <div class='input-group-btn'>");
            builder.Append("            <button type='button' title='Limpar' disabled='disabled' class='btn btn-default btn-limpar-arquivo'><i class='fa fa-times'></i></button>");
            builder.Append("            <button type='button' class='btn btn-info btn-selecionar-arquivo'><i class='fa fa-search'></i> Procurar</button>");
            builder.Append("        </div>");
            builder.Append("    </div>");
            builder.Append("</div>");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString InputSingleFile(this HtmlHelper helper, string name, string label)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class='form-group custom-file-upload'>");
            builder.Append(String.Format("    <label>{0}</label>", label));
            builder.Append("    <div class='input-group'>");
            builder.Append(String.Format("      <input style='visibility: hidden; position: absolute; top: -5000px; left: -5000px;' type='file' multiple accept='.csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel' name='{0}'>", name));
            builder.Append(String.Format("      <input type='text' class='form-control input-file-filename' name='{0}' placeholder='Nenhum arquivo selecionado...' readonly='readonly'>", name));
            builder.Append("        <div class='input-group-btn'>");
            builder.Append("            <button type='button' title='Limpar' disabled='disabled' class='btn btn-default btn-limpar-arquivo'><i class='fa fa-times'></i></button>");
            builder.Append("            <button type='button' class='btn btn-info btn-selecionar-arquivo'><i class='fa fa-search'></i> Procurar</button>");
            builder.Append("        </div>");
            builder.Append("    </div>");
            builder.Append("</div>");

            return new MvcHtmlString(builder.ToString());
        }

        public static string Flash(this HtmlHelper helper)
        {
            var message = "";
            var className = "";
            if (helper.ViewContext.TempData["success"] != null)
            {
                message = helper.ViewContext.TempData["success"].ToString();
                className = "alert-success";
            }
            else if (helper.ViewContext.TempData["warning"] != null)
            {
                message = helper.ViewContext.TempData["warning"].ToString();
                className = "alert-warning";
            }
            else if (helper.ViewContext.TempData["error"] != null)
            {
                message = helper.ViewContext.TempData["error"].ToString();
                className = "alert-danger";
            }
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(message))
            {
                sb.AppendLine("<script>");
                sb.AppendLine("$(document).ready(function() {");
                sb.AppendFormat("$('#flash-message span.mensagem').html('{0}');", HttpUtility.HtmlEncode(message));
                sb.AppendFormat("$('#flash-message').toggleClass('{0}');", className);
                sb.AppendFormat("$('#flash-message').toggleClass('{0}');", "hidden");
                sb.AppendLine("});");
                sb.AppendLine("</script>");
            }
            return sb.ToString();
        }
    }
}