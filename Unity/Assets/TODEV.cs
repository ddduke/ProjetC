/*TO DEV : 
 * COMBAT :
 * 
 * terminer formation charge useful function
 * 
 * Pour la charge hors formation: 
 * on identifie les slots dispo des enemis (fonction interne à un régiment car varie en fonction des types d'unités)
 * on identifie le nombre de tours pour atteindre chaque slot et on prend ceux qui arrivent le plus vite jusqu'à un point 
 * si 2 regiments arrivent le plus vite sur un point, alors c'est celui le plus à droite qui y va en premier 
 * si 2 régiments sont encore égaux, c'est celui qui va le plus vite
 * 
 * 
 * 
 * 
 * Attention l'attaque doit etre gérée par le régiment de manière unitaire et pas par une fonction macro (la fonction macro doit activer l'attaque mais la fonction attaque peut etre appelée par d'autres fonctions ex feat) 
 * 
 * 
 * rajouter un check pour regarder si l'unité a bien les bonus de formation : créer un formation variables lié à l'unité avec les multiplicateurs défense ou attaque ? 
 * rajouter la possibilité de flancker
 * rajouter la capacité de changer de formation = restart la fonction avec la nouvelle formation (systeme de fenetre ou l'on décide de l'emplacement également ?)
 * 
 * MAP :
 * Mettre une condition pour empecher le joueur de cliquer sur les autres POI quand l'action Moving est lancée
 * Tester la génération de map : encore des connexions incohérentes = repenser le process ? 
 * 
 */
