using System;
using _Project.Scripts.Core.Backend.Helper;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Character.Hand_Controller;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// Projectile class for ranged weapons.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// The mesh of the projectile.
        /// </summary>
        [SerializeField] private GameObject bulletMesh;

        /// <summary>
        /// The hit effect of the projectile.
        /// </summary>
        [SerializeField] private ParticleSystem hitEffect;

        /// <summary>
        /// The weapon that fired the projectile.
        /// </summary>
        private RangedWeapon _weapon;

        /// <summary>
        /// Event that is called when the projectile hits something.
        /// </summary>
        public event Action<Projectile> OnHit;

        /// <summary>
        /// Rigidbody of the projectile.
        /// </summary>
        private Rigidbody _rigidbody;

        /// <summary>
        /// Damage info of the projectile.
        /// </summary>
        private IDamageable.DamageInfo _damageInfo;

        private void Awake()
        {
            // Get the rigidbody component.
            _rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Initialize the projectile.
        /// </summary>
        /// <param name="weapon">the weapon which is shooting the projectile</param>
        public void Initialize(RangedWeapon weapon)
        {
            // Set the weapon that fired the projectile.
            _weapon = weapon;
            _damageInfo = weapon.GetDamageInfo();

            // Set the projectile to active and deactivate the hit effect.
            bulletMesh.SetActive(true);
            hitEffect.gameObject.SetActive(false);

            // Set the projectile's collision layers.
            gameObject.SetLayerRecursively(weapon.Owner.FriendlyLayerName);
            _rigidbody.excludeLayers = weapon.Owner.FriendlyLayer;

            // Set the projectile's velocity.
            _rigidbody.velocity = transform.forward * weapon.Stats.ProjectileSpeed;
        }

        private void OnCollisionEnter(Collision other)
        {
            // Check if the object that the projectile collided with is damageable.
            if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(_damageInfo);

            // Stop the projectile and deactivate the mesh.
            _rigidbody.velocity = Vector3.zero;
            bulletMesh.SetActive(false);
            hitEffect.gameObject.SetActive(true);
        }

        private void OnParticleSystemStopped()
        {
            gameObject.SetLayerRecursively("Default");
            // Projectile has completed the hit.
            OnHit?.Invoke(this);
        }
    }
}