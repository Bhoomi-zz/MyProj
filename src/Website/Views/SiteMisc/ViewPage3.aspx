<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commands.CreatePlan>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ViewPage3
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ViewPage3</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.PlanDetailsId) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.PlanDetailsId) %>
                <%: Html.ValidationMessageFor(model => model.PlanDetailsId) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.BriefNo) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.BriefNo) %>
                <%: Html.ValidationMessageFor(model => model.BriefNo) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.CreatedOn) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.CreatedOn) %>
                <%: Html.ValidationMessageFor(model => model.CreatedOn) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.HeadPlannerId) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.HeadPlannerId) %>
                <%: Html.ValidationMessageFor(model => model.HeadPlannerId) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.PlanNo) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.PlanNo) %>
                <%: Html.ValidationMessageFor(model => model.PlanNo) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Budget) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Budget) %>
                <%: Html.ValidationMessageFor(model => model.Budget) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.StartDate) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.StartDate) %>
                <%: Html.ValidationMessageFor(model => model.StartDate) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.EndDate) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.EndDate) %>
                <%: Html.ValidationMessageFor(model => model.EndDate) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ClientId) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ClientId) %>
                <%: Html.ValidationMessageFor(model => model.ClientId) %>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

