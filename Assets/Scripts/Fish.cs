using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class Fish : MonoBehaviour, IFishable
    {
        [SerializeField] float xMin, xMax, xNext;
        [SerializeField] bool isFleeing;
        static float nextXHitbox = .01f;
        [SerializeField] bool runInEditor;
        [SerializeField] Vector3 rotation;
        [SerializeField] float moveSpeed;
        [SerializeField] float fleeSpeed;
        [SerializeField] float maxFleeSpeed;
        [SerializeField] float value;
        [SerializeField] float weight;
        [SerializeField] GameObject rodEnd;
        [SerializeField] Size size;

        public void Flee()
        {
            isFleeing = true;
        }
        public void Swim()
        {
            isFleeing = false;
        }

        public void ForcePosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Initialize(float xMin, float xMax, float moveSpeed, float fleeSpeed, float maxFleeSpeed, float value, float weight)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.moveSpeed = moveSpeed;
            this.fleeSpeed = fleeSpeed;
            this.maxFleeSpeed = maxFleeSpeed;
            this.value = value;
            this.weight = weight;
            UpdatePathing();
        }


        private void Update()
        {
            if (!Application.isPlaying && !runInEditor)
                return;

            if (!isFleeing)
                CheckAndUpdatePathing();

            else
                FleePath();


            Move();
        }

        public Size GetSize()
        {
            return size;
        }
        void GetNextCoord()
        {
            xNext = Random.Range(xMin, xMax);
        }

        void CheckAndUpdatePathing()
        {

            if (System.Math.Abs(transform.position.x - xNext) > nextXHitbox)
                return;

            UpdatePathing();
        }

        void UpdatePathing()
        {

            //get the new coord
            GetNextCoord();

            //set the new direction
            TurnTowardsCoord();
        }

        void TurnTowardsCoord()
        {

            float yRotation = 0;
            if (xNext < transform.position.x)
                yRotation = - 180;

            transform.rotation = Quaternion.Euler(new Vector3(0, yRotation, 0));
        }

        void FleePath()
        {

            if (rodEnd == null)
                rodEnd = GameObject.FindGameObjectWithTag("RodTip");

            TurnToFlee();
        }

        void TurnToFlee()
        {

            Vector3 diff = transform.position - rodEnd.transform.position;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg));
        }

        void Move()
        {
            Vector3 pos = transform.position;
            int direction = pos.x < xNext ? 1 : -1;

            float movement = direction * moveSpeed * Time.deltaTime;

            if (!isFleeing)
                pos.x += movement;


            else
            {
                Vector3 diff = transform.position - rodEnd.transform.position;
                diff.Normalize();
                diff *= fleeSpeed;
                var rb = GetComponent<Rigidbody2D>();
                rb.AddForce(diff);
                if (rb.velocity.x > maxFleeSpeed)
                    rb.velocity = new Vector2(maxFleeSpeed, rb.velocity.y);
                if (rb.velocity.y > maxFleeSpeed)
                    rb.velocity = new Vector2(rb.velocity.x, maxFleeSpeed);
            }

            transform.position = pos;
        }

        private void OnValidate()
        {
            TurnTowardsCoord();
        }

        public float GetValue()
        {
            return value;
        }
        public float GetWeight()
        {
            return weight;
        }
    }
}
