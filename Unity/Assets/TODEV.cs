/*TO DEV : 
 * COMBAT : On a un systeme qui calcule le tour de l'unité, empecher tout mouvement de l'unité si le tour d'une unité n'a pas été terminé
 * je choisis a chaque tour ma target (Desactiver certains boutons UI avec tag UITour = utiliser un round manager pour récupérer l'information de savoir si toutes les unités du joueur on terminé leur déplacement)
 * ceci afin de faire un déplacement de 1 ou + cases par round pour finir le tour en 4 tours ou je selectionne de nouveau une target
 * mettre la vitesse de déplacement de la formation égale à l'unité la plus lente et mettre la vitesse en formation (nouvelle variable) des unités sur la vitesse de la formation
 * afficher les targets de chaque unité
 * integrer le systeme de tour ami / tour enemi (avec variables enemyUnit enemyFormation)
 * rajouter attaque defense range 
 * rajouter l'ordre charger (vers l'unité la plus proche) pour tester si les variables vitesses de chaque unité fonctionnent bien
 * mettre le pathfinding en A*
 * 
 * MAP :
 * Mettre une condition pour empecher le joueur de cliquer sur les autres POI quand l'action Moving est lancée
 * Tester la génération de map : encore des connexions incohérentes = repenser le process ? 
 * 
 */
