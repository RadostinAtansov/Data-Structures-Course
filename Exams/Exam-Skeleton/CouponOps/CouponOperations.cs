namespace CouponOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CouponOps.Models;
    using Interfaces;

    public class CouponOperations : ICouponOperations
    {
        Dictionary<string, Coupon> codeAndCoupons = new Dictionary<string, Coupon>();
        Dictionary<string, Website> domainAndWebSite = new Dictionary<string, Website>();

        public void AddCoupon(Website website, Coupon coupon)
        {
            if (!this.domainAndWebSite.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            domainAndWebSite[website.Domain].Coupons.Add(coupon);
            codeAndCoupons.Add(coupon.Code, coupon);
        }

        public bool Exist(Website website)
        {
            return this.domainAndWebSite.ContainsKey(website.Domain);
        }

        public bool Exist(Coupon coupon)
        {
            return this.codeAndCoupons.ContainsKey(coupon.Code);
        }

        public IEnumerable<Coupon> GetCouponsForWebsite(Website website)
        {
            if (!this.domainAndWebSite.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            return this.domainAndWebSite[website.Domain].Coupons.ToArray();
        }

        public IEnumerable<Coupon> GetCouponsOrderedByValidityDescAndDiscountPercentageDesc()
            => this.codeAndCoupons.Values.OrderByDescending(v => v.Validity).ThenByDescending(dp => dp.DiscountPercentage);

        public IEnumerable<Website> GetSites()
        {
            return this.domainAndWebSite.Values.ToList();
        }

        public IEnumerable<Website> GetWebsitesOrderedByUserCountAndCouponsCountDesc()
            => this.domainAndWebSite.Values.OrderBy(uc => uc.UsersCount).ThenByDescending(ac => ac.Coupons.Count()).ToList();

        public void RegisterSite(Website website)
        {
            if (this.domainAndWebSite.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            domainAndWebSite.Add(website.Domain, website);
        }

        public Coupon RemoveCoupon(string code)
        {
            if (!this.codeAndCoupons.ContainsKey(code))
            {
                throw new ArgumentException();
            }

            var coupon = this.codeAndCoupons[code];

            foreach (var sites in this.domainAndWebSite.Values)
            {
                var website = sites.Coupons;

                for (int i = 0; i < website.Count; i++)
                {
                    var siteCouponsCode = sites.Coupons[i];

                    if (code == siteCouponsCode.Code)
                    {
                        website.Remove(siteCouponsCode);
                    }
                }
            }

            this.codeAndCoupons.Remove(code);

            return coupon;
        }//<== see this use LINQ

        public Website RemoveWebsite(string domain)
        {
            if (!this.domainAndWebSite.ContainsKey(domain))
            {
                throw new ArgumentException();
            }
            var website = this.domainAndWebSite[domain];
            var collecstionCoupons = this.domainAndWebSite[domain].Coupons;

            while (collecstionCoupons.Count() != 0)
            {
                website.Coupons.Remove(website.Coupons.First());
            }
            this.domainAndWebSite.Remove(domain);

            return website;
        }

        public void UseCoupon(Website website, Coupon coupon)
        {
            if (!this.domainAndWebSite.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }
            if (!this.codeAndCoupons.ContainsKey(coupon.Code))
            {
                throw new ArgumentException();
            }
            var isCouponInSite = this.domainAndWebSite[website.Domain].Coupons.Contains(coupon);

            if (this.domainAndWebSite.ContainsKey(website.Domain) && !isCouponInSite)
            {
                throw new ArgumentException();
            }

            this.codeAndCoupons.Remove(coupon.Code);
            this.domainAndWebSite[website.Domain].Coupons.Remove(coupon);
        }
    }
}
