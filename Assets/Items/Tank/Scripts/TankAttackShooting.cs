﻿using Item.Ammo;
using UnityEngine;

namespace Item.Tank
{
    public class TankAttackShooting : TankAttack
    {
        public ObjectPool ammoPool;                 // 弹药池
        public Point ammoSpawnPoint;                // 发射子弹的位置

        private GameObject ammo;                    // 临时弹药

        /// <summary>
        /// 攻击实际效果
        /// </summary>
        protected override void OnAttack(params object[] values)
        {
            if (values == null || values.Length == 0)
                Launch(forceSlider.value, damage, coolDownTime);
            else if (values.Length == 1)
                Launch((float)values[0], damage, coolDownTime);
            else if (values.Length == 3)
                Launch((float)values[0], (float)values[1], (float)values[2]);
        }

        /// <summary>
        /// 发射炮弹，自定义参数变量
        /// </summary>
        /// <param name="launchForce">发射力度</param>
        /// <param name="fireDamage">伤害值</param>
        /// <param name="coolDownTime">发射后冷却时间</param>
        private void Launch(float launchForce, float fireDamage, float coolDownTime)
        {
            //获取炮弹，并发射
            ammo = ammoPool.GetNextObject(ammoSpawnPoint.GetWorldPoint(transform),false);
            ammo.GetComponent<AmmoBase>().Init(playerManager,fireDamage + additionDamage);
            ammo.GetComponent<Rigidbody>().velocity = launchForce * transform.forward;
            ammo.SetActive(true);
            cdTimer.Reset(coolDownTime);
        }

    }
}