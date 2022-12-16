using CouponOps;
using CouponOps.Models;
using System;
using VaccOps;
using VaccOps.Models;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var copOps = new CouponOperations();

            var web1 = new Website("abv.bg", 12);
            var web2 = new Website("data.bg", 13);
            var coup1 = new Coupon("abv1", 30, 111);
            var coup2 = new Coupon("data1", 30, 222);
            var coup3 = new Coupon("abv2", 30, 111);
            var coup4 = new Coupon("data2", 30, 222);

            copOps.RegisterSite(web1);
            copOps.RegisterSite(web2);
            copOps.AddCoupon(web1, coup1);
            copOps.AddCoupon(web1, coup2);
            copOps.AddCoupon(web1, coup3);
            copOps.AddCoupon(web2, coup4);
            //copOps.GetPatients();
            //copOps.ChangeDoctor(doc1, doc2, pat1);
            copOps.RemoveWebsite("abv.bg");
        }
    }
}
