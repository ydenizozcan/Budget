

using System.Collections.Generic;

namespace KazikazanButce{

public class PointEntity
{
    public int Value { get; set; }
    public int Percent{get;set;}
    public long PersonCount{get;set;}
    public static List<PointEntity> GetList()
    {
        return new List<PointEntity>(){
            new PointEntity(){Value=1,Percent=50,PersonCount=0},
            new PointEntity(){Value=5,Percent=10,PersonCount=0},        
            new PointEntity(){Value=10,Percent=5,PersonCount=0},        
            new PointEntity(){Value=15,Percent=3,PersonCount=0},        
            new PointEntity(){Value=20,Percent=2,PersonCount=0},        
            new PointEntity(){Value=25,Percent=2,PersonCount=0},        
            new PointEntity(){Value=50,Percent=1,PersonCount=0},        
            new PointEntity(){Value=100,Percent=0,PersonCount=0},        
            new PointEntity(){Value=1000,Percent=0,PersonCount=0},        
            new PointEntity(){Value=5000,Percent=0,PersonCount=0},        
            new PointEntity(){Value=10000,Percent=0,PersonCount=0},        
            new PointEntity(){Value=25000,Percent=0,PersonCount=0}    
        };

    }

}

}