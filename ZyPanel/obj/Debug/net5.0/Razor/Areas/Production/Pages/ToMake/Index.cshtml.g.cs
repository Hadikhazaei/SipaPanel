#pragma checksum "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a74705ce064aceb3c662f919b21db12a761cb0f8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ZyPanel.Areas.Production.Pages.ToMake.Areas_Production_Pages_ToMake_Index), @"mvc.1.0.razor-page", @"/Areas/Production/Pages/ToMake/Index.cshtml")]
namespace ZyPanel.Areas.Production.Pages.ToMake
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 4 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\_ViewImports.cshtml"
using HpLayer.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\_ViewImports.cshtml"
using DbLayer.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\_ViewImports.cshtml"
using ZyPanel.Helper.Utils;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\_ViewImports.cshtml"
using ZyPanel.Areas.Production.Pages.ToDo;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("RouteTemplate", "{Id}")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a74705ce064aceb3c662f919b21db12a761cb0f8", @"/Areas/Production/Pages/ToMake/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5d94f95c522c3cdd1ac22ce56577409110c0c1ca", @"/Areas/Production/Pages/_ViewImports.cshtml")]
    public class Areas_Production_Pages_ToMake_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private global::ZyPanel.Areas.Production.Pages.ToMake.Areas_Production_Pages_ToMake_Index.__Generated__PlanningInfoViewComponentTagHelper __PlanningInfoViewComponentTagHelper;
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_Alert", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "Production", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "/ToMake/Info", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
  
    ViewBag.Title = "todo produce make";
    var createUrl = Url.Page("", "Create");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "a74705ce064aceb3c662f919b21db12a761cb0f85129", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 8 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model = Model.Alert;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("model", __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n<div class=\"table-container\">\r\n    <div class=\"table-container-header\">\r\n");
#nullable restore
#line 12 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
         if (!Model.IsStopped && Model.CanCreate)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <button class=\"btn btn-outline-dark\" data-title=\"افزودن\" data-url=\'");
#nullable restore
#line 14 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                                                                          Write(createUrl);

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n            onclick=\'ajaxLoadPartialForm(this)\'>\r\n                افزودن\r\n            </button>\r\n");
#nullable restore
#line 18 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
        }
        else if (!Model.CanCreate)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"alert alert-warning w-100\" role=\"alert\">\r\n                امکان افزودن مقدور نمی باشد.\r\n            </div>\r\n");
#nullable restore
#line 24 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"alert alert-warning w-100\" role=\"alert\">\r\n                به دلیل توقف خط امکان افزودن مقدور نمی باشد.\r\n            </div>\r\n");
#nullable restore
#line 30 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n    <div class=\"table-responsive\" id=\"dataList\">\r\n        <table class=\"table table-bordered\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("vc:planning-info", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a74705ce064aceb3c662f919b21db12a761cb0f88573", async() => {
            }
            );
            __PlanningInfoViewComponentTagHelper = CreateTagHelper<global::ZyPanel.Areas.Production.Pages.ToMake.Areas_Production_Pages_ToMake_Index.__Generated__PlanningInfoViewComponentTagHelper>();
            __tagHelperExecutionContext.Add(__PlanningInfoViewComponentTagHelper);
#nullable restore
#line 34 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
__PlanningInfoViewComponentTagHelper.pid = Model.Id;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("pid", __PlanningInfoViewComponentTagHelper.pid, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
            <thead>
                <tr>
                    <th>کد قالب</th>
                    <th>کد وزن کشی</th>
                    <th>تناژ قالب (kg)</th>
                    <th>تناژ قطعه (kg)</th>
                    <th>ضایعات (kg)</th>
                    <th>گرفته شده</th>
                    <th>بارریزی شده</th>
                    <th>عملیات</th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 48 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                 if (Model.List.Any())
                {
                    foreach (var item in Model.List)
                    {
                        int wasteCount = item.TakeCount - item.PutCount;

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n                            <td>");
#nullable restore
#line 54 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                           Write(item.TemplateCode);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 55 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                           Write(item.WeightCode);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>\r\n                                خالص : ");
#nullable restore
#line 57 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                                   Write((item.TemplateWeightAsKg * item.PutCount).GetValueAsTwoPoint());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                -\r\n                                ضایع : ");
#nullable restore
#line 59 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                                   Write((item.TemplateWeightAsKg * wasteCount).GetValueAsTwoPoint());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n                                خالص : ");
#nullable restore
#line 62 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                                   Write((item.ProductWeightAsKg * item.PutCount).GetValueAsTwoPoint());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                -\r\n                                ضایع : ");
#nullable restore
#line 64 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                                   Write((item.ProductWeightAsKg * wasteCount).GetValueAsTwoPoint());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n                                ");
#nullable restore
#line 67 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                            Write(((item.TemplateWeightAsKg *
                        wasteCount)+(item.ProductWeightAsKg*wasteCount)).GetValueAsTwoPoint());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                            <td>");
#nullable restore
#line 70 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                           Write(item.TakeCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 71 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                           Write(item.PutCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td data-change>\r\n                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a74705ce064aceb3c662f919b21db12a761cb0f813970", async() => {
                WriteLiteral("\r\n                                    جزعیات\r\n                                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 73 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                                                                                   WriteLiteral(Model.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 74 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                                      WriteLiteral(item.WeightCode);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["weightcode"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-weightcode", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["weightcode"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 74 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                                                                                WriteLiteral(item.TemplateCode);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["templatecode"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-templatecode", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["templatecode"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("data-more", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                            </td>\r\n                        </tr>\r\n");
#nullable restore
#line 80 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                    }
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr data-span></tr>\r\n");
#nullable restore
#line 85 "E:\Website\SaipaCo\ZyPanel\Areas\Production\Pages\ToMake\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ZyPanel.Areas.Production.Pages.ToMake.IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ZyPanel.Areas.Production.Pages.ToMake.IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ZyPanel.Areas.Production.Pages.ToMake.IndexModel>)PageContext?.ViewData;
        public ZyPanel.Areas.Production.Pages.ToMake.IndexModel Model => ViewData.Model;
        [Microsoft.AspNetCore.Razor.TagHelpers.HtmlTargetElementAttribute("vc:planning-info")]
        public class __Generated__PlanningInfoViewComponentTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
        {
            private readonly global::Microsoft.AspNetCore.Mvc.IViewComponentHelper __helper = null;
            public __Generated__PlanningInfoViewComponentTagHelper(global::Microsoft.AspNetCore.Mvc.IViewComponentHelper helper)
            {
                __helper = helper;
            }
            [Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeNotBoundAttribute, global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewContextAttribute]
            public global::Microsoft.AspNetCore.Mvc.Rendering.ViewContext ViewContext { get; set; }
            public System.Int64 pid { get; set; }
            public override async global::System.Threading.Tasks.Task ProcessAsync(Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContext __context, Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput __output)
            {
                (__helper as global::Microsoft.AspNetCore.Mvc.ViewFeatures.IViewContextAware)?.Contextualize(ViewContext);
                var __helperContent = await __helper.InvokeAsync("PlanningInfo", ProcessInvokeAsyncArgs(__context));
                __output.TagName = null;
                __output.Content.SetHtmlContent(__helperContent);
            }
            private Dictionary<string, object> ProcessInvokeAsyncArgs(Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContext __context)
            {
                Dictionary<string, object> args = new Dictionary<string, object>();
                if (__context.AllAttributes.ContainsName("pid"))
                {
                    args[nameof(pid)] = pid;
                }
                return args;
            }
        }
    }
}
#pragma warning restore 1591
