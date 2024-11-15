﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Logic.Phy.Object;
using Bussiness;

namespace Game.Logic.Effects
{
    public class AvoidDamageEffect : BasePlayerEffect //免伤
    {
        private int m_count = 0;
        private int m_probability = 0;

        public AvoidDamageEffect(int count, int probability)
            : base(eEffectType.AvoidDamageEffect)
        {
            m_count = count;
            m_probability = probability;
        }

        public override bool Start(Living living)
        {
            AvoidDamageEffect effect = living.EffectList.GetOfType(eEffectType.AvoidDamageEffect) as AvoidDamageEffect;
            if (effect != null)
            {
                effect.m_probability = m_probability > effect.m_probability ? m_probability : effect.m_probability;
                return true;
            }
            else
            {
                return base.Start(living);
            }
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeforeTakeDamage += new LivingTakedDamageEventHandle(player_BeforeTakeDamage);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeforeTakeDamage -= new LivingTakedDamageEventHandle(player_BeforeTakeDamage);
        }

        void player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
        {
            IsTrigger = false;
            if (rand.Next(100) < m_probability)
            {
                IsTrigger = true;
                living.EffectTrigger = true;
                damageAmount = damageAmount * (1 - m_count / 100);
                living.Game.SendEquipEffect(living, LanguageMgr.GetTranslation("AvoidDamageEffect.Success"));
                //living.Game.SendAttackEffect(living, 2);
            }
        }
    }
}
