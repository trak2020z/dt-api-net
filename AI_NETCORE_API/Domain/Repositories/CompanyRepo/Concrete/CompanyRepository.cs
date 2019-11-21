﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Domain.Repositories.BaseRepo.Concrete;
using Domain.Repositories.CompanyRepo.Abstract;
using Data.Models;
using Domain.DTOToBOConverting;
using System.Linq;
using System.Timers;
using Domain.Creators.Company.Request.Abstract;
using Domain.Repositories.BaseRepo.Response;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.CompanyRepo.Concrete
{
    public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
    {
        private readonly IDTOToBOConverter _converter;
        public CompanyRepository(RepositoryContext repositoryContext, IDTOToBOConverter converter)
            : base(repositoryContext)
        {
            _converter = converter;
        }
        public RepositoryResponse<BusinessObject.Company> GetCompanyById(int id)
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            Company company = FindByCondition(comp => comp.Id == id).FirstOrDefault();
            BusinessObject.Company result = _converter.ConvertCompany(company);
            stopWatch.Stop();
            return new RepositoryResponse<BusinessObject.Company>(result, stopWatch.ElapsedMilliseconds);
        }

        public RepositoryResponse<IEnumerable<BusinessObject.Company>> GetAllCompanies()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            IQueryable<BusinessObject.Company> result = FindAll().Select(c => _converter.ConvertCompany(c));
            stopWatch.Stop();
            return new RepositoryResponse<IEnumerable<BusinessObject.Company>>(result,stopWatch.ElapsedMilliseconds);
        }

        public RepositoryResponse<BusinessObject.Company> CreateCompany(ICreateCompanyRequest createCompanyRequest)
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            Company company = new Company
            {
                Name = createCompanyRequest.Name
            };
            RepositoryContext.Companies.Add(company);
            RepositoryContext.SaveChanges();
            stopWatch.Stop();
            return new RepositoryResponse<BusinessObject.Company>(_converter.ConvertCompany(company),stopWatch.ElapsedMilliseconds);
        }

        public long ClearAll()
        {
            var tim = Stopwatch.StartNew();

            RepositoryContext.Database.ExecuteSqlCommand("DELETE FROM companies");
            RepositoryContext.SaveChanges();

            return tim.ElapsedMilliseconds;
        }
    }
}
