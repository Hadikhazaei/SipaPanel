using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.Context;
using DbLayer.DbTable;
using DbLayer.Enums;
using ZyPanel.Areas.Shared;

namespace ZyPanel.Areas.BaseInfo.Pages.Hall {
    public class IndexModel : FetchRootPage<TblHall> {
        private IWebHostEnvironment _environment;

        public IndexModel (AppDbContext context, IWebHostEnvironment environment) : base (context) {
            _environment = environment;
        }

        public class ListModel {
            public long Id { get; set; }

            public string Hall { get; set; }

            public string Line { get; set; }

            internal HallType _HelperSort { get; set; }
        }

        public List<ListModel> List { get; set; }

        public async Task OnGetAsync (int p = 1) {
            List = await _dbSet.Select (x => new ListModel {
                Id = x.Id, Hall = x.HallTitle, Line = x.Line,
                    _HelperSort = x.HallType
            }).OrderBy (x => x._HelperSort).ToListAsync ();
            // await ImportExcelToDataBase ();
        }

        private async Task ImportExcelToDataBase () {
            string wwwPath = _environment.WebRootPath;
            var filePath = Path.Combine (wwwPath, "StopData.xlsx");
            System.Text.Encoding.RegisterProvider (System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open (filePath, FileMode.Open, FileAccess.Read)) {
                using (var reader = ExcelReaderFactory.CreateReader (stream)) {
                    // 0 : کد توقف => ex : B1HHSE2
                    // 1 : نوع توقف => ex : N/A
                    // 2 : نام قسمت یا عنصر مرتبط => ex : گروه های توقف
                    // 3 : شرح توقف => ex : N/A
                    // 4 : خط تولید => ex : ex : DISA1=1, DISA2=2, BMD1=3, BMD2=4
                    var stops = new List<TblStop> ();
                    while (reader.Read ()) {
                        if (reader.Depth == 0) {
                            continue;
                        }
                        long lineId = 0;
                        var depth = reader.Depth;
                        var stop = new TblStop ();
                        stop.Code = reader.GetValue (0).ToString ();
                        stop.Title = reader.GetValue (2).ToString ();
                        stop.Brief = reader.GetValue (3).ToString ();
                        var linetTitle = reader.GetValue (4).ToString ();
                        switch (linetTitle) {
                            case "DISA1":
                                lineId = 1;
                                break;
                            case "DISA2":
                                lineId = 2;
                                break;
                            case "BMD1":
                                lineId = 3;
                                break;
                            case "BMD2":
                                lineId = 4;
                                break;
                            case "LP1":
                                lineId = 5;
                                break;
                            case "LP2":
                                lineId = 6;
                                break;
                            case "LP3":
                                lineId = 7;
                                break;
                            case "LP4":
                                lineId = 8;
                                break;
                            case "LP5":
                                lineId = 9;
                                break;
                            case "LP6":
                                lineId = 10;
                                break;
                            case "LP7":
                                lineId = 11;
                                break;
                            case "LP8":
                                lineId = 12;
                                break;
                            case "LP9":
                                lineId = 13;
                                break;
                            default:
                                break;
                        }
                        stop.TblHallId = lineId;
                        stops.Add (stop);
                        continue;
                    }
                    await _context.TblStop.AddRangeAsync (stops);
                    await _context.SaveChangesAsync ();
                }
            }
        }
    }
}