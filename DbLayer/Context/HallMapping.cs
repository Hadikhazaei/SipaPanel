using Microsoft.EntityFrameworkCore;

// 
using DbLayer.DbTable;
using DbLayer.Enums;

namespace DbLayer.Context {
    public static class HallMapping {
        public static void AddHallMapping (this ModelBuilder builder) {
            builder.Entity<TblHall> ().HasData (
                new TblHall {
                    Id = 1,
                        IsWorking = true,
                        HallType = HallType.CastIron,
                        Line = "DISA1",
                },
                new TblHall {
                    Id = 2,
                        IsWorking = true,
                        HallType = HallType.CastIron,
                        Line = "DISA2",
                },
                new TblHall {
                    Id = 3,
                        IsWorking = true,
                        HallType = HallType.CastIron,
                        Line = "BMD1",
                },
                new TblHall {
                    Id = 4,
                        IsWorking = true,
                        HallType = HallType.CastIron,
                        Line = "BMD2",
                },
                new TblHall {
                    Id = 5,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP1",
                },
                new TblHall {
                    Id = 6,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP2",
                },
                new TblHall {
                    Id = 7,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP3",
                },
                new TblHall {
                    Id = 8,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP4",
                },
                new TblHall {
                    Id = 9,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP5",
                },
                new TblHall {
                    Id = 10,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP6",
                },
                new TblHall {
                    Id = 11,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP7",
                },
                new TblHall {
                    Id = 12,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP8",
                },
                new TblHall {
                    Id = 13,
                        IsWorking = true,
                        HallType = HallType.Aluminium,
                        Line = "LP9",
                }
            );
        }
    }
}