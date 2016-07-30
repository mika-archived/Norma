using System;

// ReSharper disable VirtualMemberCallInConstructor

namespace Norma.Eta.Models.Reservations
{
    // DB 用
    public class RsvAll : Reserve
    {
        // RsvProgram, RsvTime
        public virtual DateTime? StartDate { get; set; }

        // RsvProgram
        public string ProgramId { get; set; }

        // RsvTime, RsvKeyword
        public virtual DateRange Range { get; set; }

        // RsvTime
        public RepetitionType DayOfWeek { get; set; }

        // RsvKeyword
        public string Keyword { get; set; }

        // RsvKeyword
        public bool IsRegexMode { get; set; }

        // RsvSeries
        public string SeriesId { get; set; }

        public RsvAll()
        {
            StartDate = DateTime.MinValue;
            ProgramId = "";
            Range = new DateRange {Start = DateTime.MinValue, Finish = DateTime.MaxValue};
            DayOfWeek = RepetitionType.None;
            Keyword = "";
            IsRegexMode = false;
            SeriesId = "";
        }

        // ReSharper disable PossibleInvalidOperationException
        public T Cast<T>() where T : Reserve
        {
            Reserve reserve = null;
            if (typeof(T) == typeof(RsvProgram))
            {
                reserve = new RsvProgram
                {
                    Id = Id,
                    IsEnable = IsEnable,
                    Type = Type,
                    ProgramId = ProgramId,
                    StartDate = StartDate.Value
                };
            }
            else if (typeof(T) == typeof(RsvTime))
            {
                reserve = new RsvTime
                {
                    Id = Id,
                    IsEnable = IsEnable,
                    Type = Type,
                    DayOfWeek = DayOfWeek,
                    Range = Range,
                    StartTime = StartDate.Value
                };
            }
            else if (typeof(T) == typeof(RsvKeyword))
            {
                reserve = new RsvKeyword
                {
                    Id = Id,
                    IsEnable = IsEnable,
                    Type = Type,
                    Range = Range,
                    Keyword = Keyword,
                    IsRegexMode = IsRegexMode
                };
            }
            else if (typeof(T) == typeof(RsvProgram2))
            {
                reserve = new RsvProgram2
                {
                    Id = Id,
                    IsEnable = IsEnable,
                    Type = Type,
                    ProgramId = ProgramId
                };
            }
            return (T) reserve;
        }

        // ReSharper restore PossibleInvalidOperationException

        public static RsvAll Create(Reserve reserve)
        {
            var rsvAll = new RsvAll
            {
                Id = reserve.Id,
                IsEnable = reserve.IsEnable,
                Type = reserve.Type
            };
            if (reserve is RsvProgram)
            {
                var program = (RsvProgram) reserve;
                rsvAll.ProgramId = program.ProgramId;
                rsvAll.StartDate = program.StartDate;
            }
            else if (reserve is RsvTime)
            {
                var time = (RsvTime) reserve;
                rsvAll.Range = time.Range;
                rsvAll.StartDate = time.StartTime;
                rsvAll.DayOfWeek = time.DayOfWeek;
            }
            else if (reserve is RsvKeyword)
            {
                var keyword = (RsvKeyword) reserve;
                rsvAll.Range = keyword.Range;
                rsvAll.Keyword = keyword.Keyword;
                rsvAll.IsRegexMode = keyword.IsRegexMode;
            }
            else if (reserve is RsvProgram2)
            {
                var program = (RsvProgram2) reserve;
                rsvAll.ProgramId = program.ProgramId;
            }
            else if (reserve is RsvSeries)
            {
                var series = (RsvSeries) reserve;
                rsvAll.SeriesId = series.SeriesId;
            }
            return rsvAll;
        }
    }
}