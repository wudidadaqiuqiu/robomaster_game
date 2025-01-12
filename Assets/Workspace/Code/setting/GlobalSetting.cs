using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace setting
{
    #region 数据结构
    public struct Sensitivity
    {
        public float hor;
        public float ver;
    };

    public struct BulletParam
    {
        public float bullet17mm_speed;
        public float bullet17mm_delta_time;
    };

    public struct HeroParam
    {

    }

    public struct EngineerParam
    {

    }

    public struct InfantryParam
    {

    }

    public struct SentryParam
    {

    }
    #endregion

    public class GlobalSetting : MonoBehaviour
    {
        public GlobalSetting Instance;

        public Sensitivity sensitivity;
        public BulletParam bullet_param;
        public HeroParam hero_param;
        public EngineerParam engineer_param;
        public InfantryParam infantry_param;
        public SentryParam sentry_param;

        private void Awake()
        {
            // sensitivity
            sensitivity.hor = 0.5f;
            sensitivity.ver = 0.5f;

            // bullet_param
            bullet_param.bullet17mm_speed = 30.0f;
            bullet_param.bullet17mm_delta_time = 0.04f;

            Instance = this;
        }
    }
};
