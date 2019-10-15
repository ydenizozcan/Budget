using System;
using System.Collections.Generic;
using System.Linq;

namespace KazikazanButce{

    public class BudgetDistribution
    {
        private long _targetGroup;
        private long _budget;
        private List<PointEntity> _points;
        private List<PointEntity> _minPoints;
        private long _remainingBudget;
        private long _remainingCard;

        public BudgetDistribution(long targetGroup,long budget)
        {
            _targetGroup=targetGroup;
            _budget=budget;
            _points = PointEntity.GetList();
            _remainingBudget=_budget;
            _remainingCard = _targetGroup;
        }
        public void Start(){
           //bütçe alt sınır kontrol 
            if(!(this.BudgetLowerLimitControl()))
                Environment.Exit(0);

            //bütçe dağıtım yüzdeleri
            this.ShowBudgetDistributionAlgorithm();
            //bütçe dağıtım 
            this.StartDistributionJob();
            //console yazdırma 
            this.ShowBudgetDistributionResult();
            //bütçe dağıtım kontrol 
            this.ShowBudgetDistributionControl();
        }
        private bool BudgetLowerLimitControl()
        {
            long lowerLimit = 0 ;
            foreach(var point in _points){
                if(point.Percent==0){
                    lowerLimit+=point.Value;
                }
                else{
                    lowerLimit+=point.Value*(long)Math.Ceiling((decimal)_targetGroup*point.Percent/100);
                }
            }

            Console.WriteLine($"Hedeflenen {_targetGroup} sayısı için minimum {lowerLimit} bütçe belirlenmelidir.");
            if(lowerLimit>_budget){
                Console.WriteLine($"Belirlediğiniz {_budget} değerindeki bütçe minimum belirlenmesi gereken bütçenin altındadır.");
                return false;
            }
            return true;


        }
        private void ShowBudgetDistributionAlgorithm(){
           _points.ForEach(x=>Console.WriteLine($"{x.Value}-{x.Percent}%"));
           Console.WriteLine("Bütçe dağıtımı başlatıldı...");
        }
        private void StartDistributionJob()
        {
           //ilk adıda minimum düzeyde puan değerleri atanır.
            this.MinimumDistribution();
            //ikinci adımda random atamalar gerçekleştirilir.
            this.RandomDistribution();
        }
        public void MinimumDistribution(){
           
            foreach(var point in _points){
                if(point.Percent==0){ point.PersonCount = 1; }
                else {
                    long _personCount = (long)(Math.Ceiling((double)((_targetGroup *point.Percent)/100)));
                    point.PersonCount += _personCount;
                }
                _remainingBudget -= point.PersonCount*point.Value;
                _remainingCard -= point.PersonCount;
            } 

            _minPoints = new List<PointEntity>();
            _points.ForEach(x=>{
                _minPoints.Add(new PointEntity(){
                    Value=x.Value,
                    PersonCount=x.PersonCount
                });
            });
           

        }
        public void RandomDistribution(){
            //Dağıtılacak kart sayısı , bütçeden büyük mü ? 
            //Küçükse
            if(_remainingBudget<_remainingCard){
                Console.WriteLine("Yeteri kadar bütçe bulunamamıştır.");
            }
            //Büyükse kart sayısı kadar 1 puan eklenir. Ve o puanlar arasında dönülüp bütçe sıfırlanana kadar devam edilir. 
            else{
                _points.FirstOrDefault(x=>x.Value==1).PersonCount+=_remainingCard;
                _remainingBudget-= _remainingCard*1;
              
                 var _mod = _remainingBudget%5;
                 _points.FirstOrDefault(x=>x.Value==1).PersonCount+=_mod;
                 _remainingBudget-=_mod;

                
                while(_remainingBudget>0){
                    var veriablePoint = SetVeriablePoint();
                    var randomPoints = _points.Where(x=>x.Value>veriablePoint && (5*x.Value)<=_remainingBudget).Select(x=>x.Value).ToList();
                  
                    if(randomPoints==null || randomPoints.Count()==0){
                        if(_remainingBudget%5!=0) break;
                        var k = _remainingBudget/5;
                        _points.FirstOrDefault(x=>x.Value==5).PersonCount-=k;
                        _points.FirstOrDefault(x=>x.Value==10).PersonCount+=k;
                        _remainingBudget+=5*k;
                        _remainingBudget-=10*k;
                        continue;
                    }

                    var index = (new Random()).Next(0,randomPoints.Count());
                    var randomPoint = randomPoints[index];         
                    _points.FirstOrDefault(x=>x.Value==randomPoint).PersonCount+=5;
                    _points.FirstOrDefault(x=>x.Value==veriablePoint).PersonCount -=5;
                    _remainingBudget-=(5*randomPoint);        
                    _remainingBudget+=(5*veriablePoint); 
                }
                var totalCard  = _points.Sum(x=>x.PersonCount);
                if((_targetGroup-totalCard)<0){
                    var k = Math.Abs(_targetGroup-totalCard);
                    switch(k){
                        case 1:
                            _points.FirstOrDefault(x=>x.Value==5).PersonCount-=2;
                            _points.FirstOrDefault(x=>x.Value==10).PersonCount+=1;
                        break;
                        case 2:
                            _points.FirstOrDefault(x=>x.Value==5).PersonCount-=3;
                            _points.FirstOrDefault(x=>x.Value==15).PersonCount+=1;
                        break;
                        case 3:
                            _points.FirstOrDefault(x=>x.Value==5).PersonCount-=4;
                            _points.FirstOrDefault(x=>x.Value==20).PersonCount+=1;
                        break;
                        case 4:
                            _points.FirstOrDefault(x=>x.Value==5).PersonCount-=5;
                            _points.FirstOrDefault(x=>x.Value==25).PersonCount+=1;
                        break;
                        default:
                        break;
                    }
                }


            }
        }

        private int SetVeriablePoint()
        {
            var points = _minPoints.Select(x=>x.Value).OrderBy(x=>x);
        
             foreach(var point in points){
                if(_points.FirstOrDefault(x=>x.Value==point).PersonCount-5>=_minPoints.FirstOrDefault(x=>x.Value==point).PersonCount)
                    return point;
             }
             return 1;
        }

        private void ShowBudgetDistributionResult()
        {
           _points.ForEach(x=>Console.WriteLine($"{x.Value}-{x.Percent}%-{x.PersonCount}"));
           Console.WriteLine("Bütçe dağıtımı tamamlandı.Kontrol işlemi yapılıyor...");
        }
        private void ShowBudgetDistributionControl()
        {
           var totalPoint =  _points.Sum(x=>x.PersonCount*x.Value);
           if(totalPoint==_budget)
            Console.WriteLine($"{totalPoint} olarak belirlediğiniz bütçe tamamen dağıtılmıştır.");
           else{
            Console.WriteLine($"{_budget-totalPoint} değerinde bütçe dağıtılamamıştır.");
            Console.WriteLine($"Toplam dağıtılan bütçe {totalPoint} kadardır.");
            }

           var totalCard = _points.Sum(x=>x.PersonCount);
           if(totalCard==_targetGroup)
            Console.WriteLine($"{totalCard} olarak hedeflenen kullanıcı sayısı kadar kart dağıtımı yapılmıştır.");
           else{
            Console.WriteLine($"{_targetGroup-totalCard} kadar kart dağıtımı gerçekleşmemiştir");
            Console.WriteLine($"Toplam basılan kart sayısı {totalCard} kadardır.");
            }
        }

    }



}