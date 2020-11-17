using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    AudioSource audioSource;
    public ParticleSystem smokeEffect;
    
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    public AudioClip robotFixed;
    Animator animator;

    private RubyController rubyController;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController");
        if (rubyControllerObject != null)
        {
            rubyController = rubyControllerObject.GetComponent<RubyController>();
            print ("Found the RubyConroller Script!");
        }
        if (rubyController == null)
        {
            print ("Cannot find GameController Script!");
        }
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource= GetComponent<AudioSource>();
     
    }
        public void PlaySound(AudioClip clip)
    {
    audioSource.PlayOneShot(clip);
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {

        if (rubyController != null)
        {
            rubyController.ChangeHealth(-1);
        }
    }
    
    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        //optional if you added the fixed animation

        if (rubyController != null)
        {
            print("Found the RubyController on a hit!");
        }
        animator.SetTrigger("Fixed");
        print ("I should be fixed right now!");
         {

            PlaySound(robotFixed);
        }
        smokeEffect.Stop();
    }
}