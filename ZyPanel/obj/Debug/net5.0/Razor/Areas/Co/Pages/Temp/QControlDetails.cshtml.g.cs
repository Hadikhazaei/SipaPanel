#pragma checksum "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e32a6da3b3afa3a9ed85fcd06c71544fee05229f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ZyPanel.Areas.Co.Pages.Temp.Areas_Co_Pages_Temp_QControlDetails), @"mvc.1.0.razor-page", @"/Areas/Co/Pages/Temp/QControlDetails.cshtml")]
namespace ZyPanel.Areas.Co.Pages.Temp
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
#line 4 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\_ViewImports.cshtml"
using CldLayer.Persian;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\_ViewImports.cshtml"
using HpLayer.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\_ViewImports.cshtml"
using DbLayer.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\_ViewImports.cshtml"
using ZyPanel.Areas.Co.Pages.Hall;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("RouteTemplate", "{productid}/{begindate}/{finishdate}")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e32a6da3b3afa3a9ed85fcd06c71544fee05229f", @"/Areas/Co/Pages/Temp/QControlDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4f057d139861c340e4c98107a254ca3d24b64187", @"/Areas/Co/Pages/_ViewImports.cshtml")]
    public class Areas_Co_Pages_Temp_QControlDetails : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_Pagination", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
  
    ViewBag.Title = "گزارش کنترل کیفی";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"table-container my-3\">\r\n    <div class=\"table-responsive\" id=\"dataList\">\r\n        <table class=\"table table-bordered table-striped\">\r\n            <caption>\r\n                از : <bdo class=\"ltr\">");
#nullable restore
#line 11 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                                 Write(Model.BeginDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</bdo> ،\r\n                تا : <bdo class=\"ltr\"> ");
#nullable restore
#line 12 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                                  Write(Model.FinishDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</bdo> ،\r\n                <span>");
#nullable restore
#line 13 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                 Write(Model.Hall);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span> ، <span>");
#nullable restore
#line 13 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                                            Write(Model.Product);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
            </caption>
            <thead>
                <tr>
                    <th>#</th>
                    <th>تاریخ ایجاد</th>
                    <th>تاریخ تولید</th>
                    <th>عیب</th>
                    <th>محل عیب</th>
                    <th>ضایع</th>
                    <th>سالم</th>
                    <th>درصد ضایع</th>
                    <th>مرجوعی</th>
                    <th>نوع ضایع</th>
                    <th>نامنطبق</th>
                    <th>ایستگاه</th>
                    <th>کد ردیابی</th>
                    <th>نوع محافظ</th>
                    <th>بازرس</th>
                    <th>توضیحات</th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 36 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                 if (Model.List.Any())
                {
                    foreach (var item in Model.List)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n                            <td>");
#nullable restore
#line 41 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 42 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.CreateDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 43 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.ProductionDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 44 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.DefectTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 45 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.DefectPlaceTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 46 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.Waste);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 47 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.Healthy);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 48 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.WastePercent);

#line default
#line hidden
#nullable disable
            WriteLiteral(" %</td>\r\n                            <td>");
#nullable restore
#line 49 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.BackCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 50 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.BackTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td data-ready=\"");
#nullable restore
#line 51 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                                       Write(item.IsWaste);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"></td>\r\n                            <td>");
#nullable restore
#line 52 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.StationTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 53 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.TrackCode);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 54 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.ShieldType);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 55 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                           Write(item.Inspecter);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td data-text>\r\n                                <a href=\"javascript:void(0)\" data-title=\"توضیحات\" data-body=\"");
#nullable restore
#line 57 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                                                                                        Write(item.Explanation);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"\r\n                            onclick=\"viewModalAsText(this)\">\r\n                                    <i class=\"bi\"></i>\r\n                                </a>\r\n                            </td>\r\n                        </tr>\r\n");
#nullable restore
#line 63 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                    }
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr data-span></tr>\r\n");
#nullable restore
#line 68 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "e32a6da3b3afa3a9ed85fcd06c71544fee05229f12439", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 71 "C:\Users\A V V A L Pc\Desktop\SaipaCo\ZyPanel\Areas\Co\Pages\Temp\QControlDetails.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model = (Model.List.TotalPages);

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("model", __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.SingleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ZyPanel.Areas.Co.Pages.Temp.QControlDetailsModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ZyPanel.Areas.Co.Pages.Temp.QControlDetailsModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ZyPanel.Areas.Co.Pages.Temp.QControlDetailsModel>)PageContext?.ViewData;
        public ZyPanel.Areas.Co.Pages.Temp.QControlDetailsModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
