using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using FeatureLoggerService.Entities;
using FeatureLoggerService.Helpers;
using FeatureLoggerService.Repositories;

namespace FeatureLoggerService.Services
{
    [ServiceContract]
    public interface IFeatureLogService
    {
        [OperationContract]
        bool AddFeatureModifyLog(ModificationInfo modifyInfo);

        [OperationContract]
        [ProxyDataResolve]
        ModificationInfoDTO GetFeatureModifyInfos(long featureFid, String user, ModifyState state, String featureClass, int skipCount, int takeCount);

        [OperationContract]
        [ProxyDataResolve]
        ModificationInfoDTO GetFeatureModifyInfosInPeriod(long featureFid, String user, ModifyState state, String featureClass, DateTime dateFrom, DateTime dateTo, int skipCount, int takeCount);

        [OperationContract]
        [ProxyDataResolve]
        List<SemanticsModificationInfo> GetSemanticsModificationInfo(long modificationInfoId);

        [OperationContract]
        String GetGeometryModificationInfo(long modificationInfoId);

        [OperationContract]
        List<String> GetUsers();
        
        [OperationContract]
        List<String> GetFeatureClasses();
    }

    public class FeatureLogService : IFeatureLogService
    {
        private readonly IRepository<ModificationInfo> _repositoryModificationInfo;
        private readonly IRepository<SemanticsModificationInfo> _repositorySemanticsInfo;
        private readonly IRepository<GeometryModificationInfo> _repositoryGeometryInfo;
        
        public FeatureLogService(IRepository<ModificationInfo> repositoryModificationInfo,
                                 IRepository<SemanticsModificationInfo> repositorySemanticsInfo,
                                 IRepository<GeometryModificationInfo> repositoryGeometryInfo)
        {
            _repositoryModificationInfo = repositoryModificationInfo;
            _repositorySemanticsInfo = repositorySemanticsInfo;
            _repositoryGeometryInfo = repositoryGeometryInfo;
        }

        public bool AddFeatureModifyLog(ModificationInfo modifyInfo)
        {
            _repositoryModificationInfo.Add(modifyInfo);
            Console.WriteLine("{0}, {1}, {2}, {3}", modifyInfo.UserName, modifyInfo.State, modifyInfo.FeatureClass, modifyInfo.FID);
            return _repositoryModificationInfo.SaveChanges() > 0;
        }

        public List<ModificationInfo> GetFeatureModifyInfosByFeatureFid(long featureFid)
        {
            var modifyInfos = _repositoryModificationInfo.FindAll(x => x.FID == featureFid);
            modifyInfos = modifyInfos.OrderByDescending(x => x.ID);

            return modifyInfos.ToList();
        }

        public ModificationInfo GetFeatureModifyInfo(long featureFid)
        {
            return _repositoryModificationInfo.FindOne(x => x.FID == featureFid);
        }

        public ModificationInfoDTO GetFeatureModifyInfos(long featureFid, String user, ModifyState state, String featureClass, int skipCount, int takeCount)
        {
            var modifyInfos = string.IsNullOrEmpty(user) ? _repositoryModificationInfo.FindAll() : _repositoryModificationInfo.FindAll(x=>x.UserName == user);

            if (featureFid > 0)
            {
                modifyInfos = modifyInfos.Where(x => x.FID == featureFid);
            }

            if (state != ModifyState.None)
            {
                modifyInfos = modifyInfos.Where(x => x.State == state);
            }

            if (!String.IsNullOrEmpty(featureClass))
            {
                modifyInfos = modifyInfos.Where(x => x.FeatureClass == featureClass);
            }

            var count = modifyInfos.Count();
            modifyInfos = modifyInfos.OrderByDescending(x => x.ID)
                                     .Skip(skipCount)
                                     .Take(takeCount);
            return new ModificationInfoDTO
            {
                TotalCount = count,
                Infos = modifyInfos.ToList()
            };
        }

        public ModificationInfoDTO GetFeatureModifyInfosInPeriod(long featureFid, String user, ModifyState state, String featureClass, DateTime dateFrom, DateTime dateTo, int skipCount, int takeCount)
        {
            var modifyInfos = _repositoryModificationInfo.FindAll(x => x.ModifyTime <= dateTo && x.ModifyTime >= dateFrom);

            if (featureFid > 0)
            {
                modifyInfos = modifyInfos.Where(x => x.FID == featureFid);
            }

            if (!string.IsNullOrEmpty(user))
            {
                modifyInfos = modifyInfos.Where(x => x.UserName == user);
            }

            if (state != ModifyState.None)
            {
                modifyInfos = modifyInfos.Where(x => x.State == state);
            }

            if (!String.IsNullOrEmpty(featureClass))
            {
                modifyInfos = modifyInfos.Where(x => x.FeatureClass == featureClass);
            }

            var count = modifyInfos.Count();
            modifyInfos = modifyInfos.OrderByDescending(x => x.ID)
                                     .Skip(skipCount)
                                     .Take(takeCount);
            return new ModificationInfoDTO
            {
                TotalCount = count,
                Infos = modifyInfos.ToList()
            };
        }

        public List<SemanticsModificationInfo> GetSemanticsModificationInfo(long modificationInfoId)
        {
            var semanticModifyInfos = _repositorySemanticsInfo.FindAll(x => x.Info.ID == modificationInfoId);
            return semanticModifyInfos.ToList();
        }

        public String GetGeometryModificationInfo(long modificationInfoId)
        {
            var geometryModificationInfo = _repositoryGeometryInfo.FindOne(x => x.Info.ID == modificationInfoId);
            return geometryModificationInfo != null ? geometryModificationInfo.WKTGeometry : String.Empty;
        }

        public List<string> GetUsers()
        {
            return _repositoryModificationInfo.FindAll().Select(x => x.UserName).Distinct().ToList();
        }

        public List<string> GetFeatureClasses()
        {
            return _repositoryModificationInfo.FindAll().Select(x => x.FeatureClass).Distinct().ToList();
        }
    }
}
 