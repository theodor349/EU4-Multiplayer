using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EU4_Target_Selector.Models
{
    public class Province
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string SuperRegion { get; set; }

        public Province(int id, string area)
        {
            Id = id;
            Area = area;
            Region = Models.Region.Regions.First(x => x.Areas.Contains(area)).Name;
            SuperRegion = Models.SuperRegion.SuperRegions.First(x => x.Regions.Contains(Region)).Name;
        }
    }

    public class Area
    {
        public string Name { get; set; }
        public List<int> Provinces { get; set; }

        public Area(string text)
        {
            Name = AreaNameReg.Match(text).Value;
            var temp = ProvinceLineReg.Match(text).Value;
            Provinces = ProvinceNameReg.Matches(temp).ToList().ConvertAll(x => int.Parse(x.Value));
        }

        public static Regex AreaNameReg = new Regex(@"([a-z]|_)+_area");
        public static Regex ProvinceLineReg = new Regex(@"\t(([0-9])+| )+");
        public static Regex ProvinceNameReg = new Regex(@"[0-9]+");
        public static Regex AreaReg = new Regex(@"([a-z]|_)+_area = {.*\n(\s)*([0-9]|\s)+\n}");
        public static List<Area> Areas = new List<Area>();
        public static void LoadAreas(string path)
        {
            var text = File.ReadAllText(path);
            var matches = AreaReg.Matches(text);
            foreach (Match m in matches)
            {
                Areas.Add(new Area(m.Value));
            }
        }
    }

    public class Region
    {
        public string Name { get; set; }
        public List<string> Areas { get; set; } = new();
        
        public static Regex RegionNameReg = new Regex(@"([a-z]|_)+_region");
        public static Regex AreaNameReg = new Regex(@"([a-z]|_)+_area");
        public Region(string text)
        {
            Name = RegionNameReg.Match(text).Value;
            Areas = AreaNameReg.Matches(text).ToList().ConvertAll(x => x.Value);
        }

        public static Regex RegionReg = new Regex(@"([a-z]|_)+_region = {\n\tareas = {(\n\t\t([a-z]|_|\s)+_area)+\n\t}\n}");
        public static List<Region> Regions = new List<Region>();
        public static void LoadRegions(string path)
        {
            var text = File.ReadAllText(path);
            var matches = RegionReg.Matches(text);
            foreach (Match m in matches)
            {
                Regions.Add(new Region(m.Value));
            }
        }
    }

    public class SuperRegion
    {
        public string Name { get; set; }
        public List<string> Regions { get; set; } = new();

        public static Regex SuperRegionNameReg = new Regex(@"([a-z]|_)+_superregion");
        public static Regex RegionNameReg = new Regex(@"([a-z]|_)+_region");
        public SuperRegion(string text)
        {
            Name = SuperRegionNameReg.Match(text).Value;
            Regions = RegionNameReg.Matches(text).ToList().ConvertAll(x => x.Value);
        }

        public static Regex SuperRegionReg = new Regex(@"([a-z]|_)+_superregion = {(\n\t([a-z]|_|\s)+_region)+\n}");
        public static List<SuperRegion> SuperRegions = new List<SuperRegion>();
        public static void LoadSuperRegions(string path)
        {
            var text = File.ReadAllText(path);
            var matches = SuperRegionReg.Matches(text);
            foreach (Match m in matches)
            {
                SuperRegions.Add(new SuperRegion(m.Value));
            }
        }
    }
}
