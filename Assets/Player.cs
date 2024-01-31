using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	private Vector2 velocity;
	public Animator animator;
	public SpriteRenderer playerSpriteRenderer;
	public Text ñoinCounter;
	public int score;

	private float gravity = -200;
	private float maxVelocity = 100;
	private float maxAcceleration = 5;
	private float acceleration = 5;
    public float distance = 0;
	private float jumpVelocity = 10;
	private float groundHeight = -1;
	public bool isGrounded = false;

	public bool isHoldingJump = false;
	private float maxHoldJumpTime = 0.4f;
	private float holdJumpTimer = 0.0f;
	private float jumpGroundThreshold = 1;

	private float raycastDistanceY = 1.0f;
	private float raycastDistanceZ = 0.6f;
	public bool isRaycastEnabled = true;
	public bool isInvincible = false;
	private float invincibilityDuration = 5f;
	private float invincibilityTimer = 0f;

	public bool isDead = false;

	void Update()
    {
		Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
			}
        }
		if (Input.GetKeyUp(KeyCode.Space)) isHoldingJump = false;

		Vector3 currentPosition = transform.position;

		if (isGrounded && Input.GetButton("Duck"))
        {
			animator.SetBool("Duck", true);
			currentPosition.y = -1.44f;
			transform.position = currentPosition;
			
			if (!isInvincible)
			{
				raycastDistanceY = 0.5f;
				isRaycastEnabled = true;
			}

			if (isHoldingJump && !isInvincible)
            {
				animator.SetBool("Duck", false);
				raycastDistanceY = 1.0f;
				isRaycastEnabled = true;
			}
			else if (isHoldingJump && isInvincible)
			{
				animator.SetBool("Duck", false);
				isRaycastEnabled = false;
			}
		}
        if (Input.GetButtonUp("Duck"))
        {
			animator.SetBool("Duck", false);
			currentPosition.y = -1f;
			transform.position = currentPosition;
			if (!isInvincible)
			{
				raycastDistanceY = 1.0f;
				isRaycastEnabled = true;
			}
		}

		Vector2[] raycastDirectionsY = { Vector2.up, Vector2.down };
		Vector2[] raycastDirectionsZ = { Vector2.right, Vector2.left };

		if (isRaycastEnabled)
		{
			foreach (Vector2 direction in raycastDirectionsY)
			{
				RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistanceY);
				Debug.DrawRay(transform.position, direction * raycastDistanceY, Color.red);

				if (hit.collider != null)
				{
					if (hit.collider.CompareTag("enemy"))
					{
						Debug.Log("Ïðîèçîøëî ñòîëêíîâåíèå ñ ïðåïÿòñòâèåì: " + hit.collider.name);
						isDead = true;
					}

					if (hit.collider.CompareTag("coin")) CollectCoin(hit.collider.gameObject);

					if (hit.collider.CompareTag("upgrade")) CollectUpgrade(hit.collider.gameObject);
				}
			}

			foreach (Vector2 direction in raycastDirectionsZ)
			{
				RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistanceZ);
				Debug.DrawRay(transform.position, direction * raycastDistanceZ, Color.red);

				if (hit.collider != null)
				{
					if (hit.collider.CompareTag("enemy"))
					{
						isDead = true;
					}

					if (hit.collider.CompareTag("coin")) CollectCoin(hit.collider.gameObject);

					if (hit.collider.CompareTag("upgrade")) CollectUpgrade(hit.collider.gameObject);
				}
			}
		}
		
		if (isInvincible)
		{
			invincibilityTimer += Time.deltaTime;
			if (invincibilityTimer >= invincibilityDuration)
			{
				isInvincible = false;
				invincibilityTimer = 0f;

				Color currentColor = playerSpriteRenderer.color;
				currentColor.a = 1f;
				playerSpriteRenderer.color = currentColor;
				
				if (isGrounded && Input.GetButton("Duck"))
				{
					raycastDistanceY = 0.5f;
					isRaycastEnabled = true;
				}
				else
				{
					raycastDistanceY = 1.0f;
					isRaycastEnabled = true;
				}
			}
		}
	}

	void CollectCoin(GameObject coin)
    {
        Destroy(coin);
		score += 1;
		ñoinCounter.text = score.ToString();
	}

	void CollectUpgrade(GameObject upgrade)
	{
		Destroy(upgrade);
		StartInvincibility();
	}

	public void StartInvincibility()
	{
		isInvincible = true;
		invincibilityTimer = 0f;

		Color currentColor = playerSpriteRenderer.color;
		currentColor.a = 0.5f;
		playerSpriteRenderer.color = currentColor;
		isRaycastEnabled = false;
	}

	private void FixedUpdate()
	{
        Vector2 pos = transform.position;
        
        if (!isGrounded)
        {
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
				if (holdJumpTimer >= maxHoldJumpTime) isHoldingJump = false;
            }
            
            pos.y += velocity.y * Time.fixedDeltaTime;
			if (!isHoldingJump) velocity.y += gravity * Time.fixedDeltaTime;

            if (pos.y <= groundHeight)
			{
			    pos.y = groundHeight;
			    isGrounded = true;
			    holdJumpTimer = 0;
			}
		}

		if (!isDead) distance += velocity.x * Time.fixedDeltaTime;
			
        if (isGrounded)
        {
			float velocityRatio = velocity.x / maxVelocity;
			acceleration = maxAcceleration * (1 - velocityRatio);

            velocity.x += acceleration * Time.fixedDeltaTime;
			if (velocity.x >= maxVelocity) velocity.x = maxVelocity;
        }

		transform.position = pos;
	}
}
