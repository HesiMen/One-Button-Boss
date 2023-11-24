using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteOnSpawn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> spriteCollection;
    
    private void Awake()
    {
        spriteRenderer.sprite = spriteCollection[Random.Range(0, spriteCollection.Count)];
    }
}
