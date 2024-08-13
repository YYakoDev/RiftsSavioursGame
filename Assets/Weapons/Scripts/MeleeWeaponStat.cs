[System.Serializable]public class MeleeWeaponStat
    {
        public float _cooldown, _pullForce, _atkRange, _atkSpeed, _knockbackForce, _damageDelay, _criticalDamageMultiplier, _pullDuration;
        public int _atkDmg, _criticalChance, _staminaConsumption;
        public MeleeWeaponStat
        (float cooldown, float pullForce, float atkRange, float atkSpeed, float knockbackForce, float delay, float criticalDamageMultiplier, float pullDuration, int atkDmg, int criticalChance, int staminaConsumption)
        {
            _cooldown = cooldown;
            _pullForce = pullForce;
            _pullDuration = pullDuration;
            _atkRange = atkRange;
            _atkSpeed = atkSpeed;
            _knockbackForce = knockbackForce;
            _damageDelay = delay;
            _atkDmg = atkDmg;
            _criticalDamageMultiplier = criticalDamageMultiplier;
            _criticalChance = criticalChance;
            _staminaConsumption = staminaConsumption;
        }
        public MeleeWeaponStat(MeleeWeaponStat statsToInherit)
        {
            this.GetStats(statsToInherit);
        }

        public void GetStats(MeleeWeaponStat referenceClass)
        {
            //make this class inherit the stats of the reference class
            _cooldown = referenceClass._cooldown;
            _pullForce = referenceClass._pullForce;
            _atkRange = referenceClass._atkRange;
            _atkSpeed = referenceClass._atkSpeed;
            _knockbackForce = referenceClass._knockbackForce;
            _damageDelay = referenceClass._damageDelay;
            _criticalDamageMultiplier = referenceClass._criticalDamageMultiplier;
            _pullDuration = referenceClass._pullDuration;
            _atkDmg = referenceClass._atkDmg;
            _criticalChance = referenceClass._criticalChance;
            _staminaConsumption = referenceClass._staminaConsumption;

        }

        public void Add(MeleeWeaponStat referenceClass)
        {
            //add the stats of the reference class to this class
            _cooldown += referenceClass._cooldown;
            _pullForce += referenceClass._pullForce;
            _atkRange += referenceClass._atkRange;
            _atkSpeed += referenceClass._atkSpeed;
            _knockbackForce += referenceClass._knockbackForce;
            _damageDelay += referenceClass._damageDelay;
            _criticalDamageMultiplier += referenceClass._criticalDamageMultiplier;
            _pullDuration += referenceClass._pullDuration;
            _atkDmg += referenceClass._atkDmg;
            _criticalChance += referenceClass._criticalChance;
            _staminaConsumption += referenceClass._staminaConsumption;
            
        }

        public void Subtract(MeleeWeaponStat referenceClass)
        {
            //subtract the stats of the reference class from this class
            _cooldown -= referenceClass._cooldown;
            _pullForce -= referenceClass._pullForce;
            _atkRange -= referenceClass._atkRange;
            _atkSpeed -= referenceClass._atkSpeed;
            _knockbackForce -= referenceClass._knockbackForce;
            _damageDelay -= referenceClass._damageDelay;
            _criticalDamageMultiplier -= referenceClass._criticalDamageMultiplier;
            _pullDuration -= referenceClass._pullDuration;
            _atkDmg -= referenceClass._atkDmg;
            _criticalChance -= referenceClass._criticalChance;
            _staminaConsumption -= referenceClass._staminaConsumption;
        }
    }
