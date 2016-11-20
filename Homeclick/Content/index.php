<?php
get_header(); 
?>
<main id="main" class="site-main">
    <div class="container">
        <div class="row">
            <div class="choose_slider">
                <div class="choose_slider_items">
                    <ul id="slider-home">
                        <?php
                        $featured = new WP_Query('posts_per_page=-1&cat=3&orderby=rand');

                        while($featured->have_posts()) : $featured->the_post();
                            ?>
                            <li class="current_item">
                                <a href="<?php the_permalink(); ?>">
                                    <?php the_post_thumbnail() ?>
                                </a>
                            </li>
                            <?php
                        endwhile;
                        wp_reset_postdata();
                        ?>
                    </ul>
                    <div class="pavination-slider " style="display:none;">
                        <a id="btn_next2" href="#"><i class="fa fa-angle-left"></i></a>
                        <a id="btn_prev2" href="#"><i class="fa fa-angle-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="about-home">
                <p><?php echo get_field('about_home','option') ?></p>
            </div>
            <div class="home-dichvu">
                <h3 class="title-home title-home-dichvu">Dịch vụ của chúng tôi</h3>
                <div id="home-dichvu" class=" owl-theme">
                    <?php
                    $taxonomy ='dichvu_categories';
                    $tax_terms = get_terms($taxonomy,array( 'parent' => 0 ));

                    foreach ($tax_terms as $tax_term) {
						$image_attributes = wp_get_attachment_image_src(get_term_thumbnail_id($tax_term->term_id), 'full');
                        $params = array('width' => 800, 'height' => 500);
                        $img_category = bfi_thumb($image_attributes[0], $params);
                        ?>
                        <div class="item">
                            <a href="<?php echo  esc_attr(get_term_link($tax_term, $taxonomy)) ?>" ><img src="<?php echo $img_category ?>" alt=""/></a>
                            <a class="title-item" href="<?php echo  esc_attr(get_term_link($tax_term, $taxonomy)) ?>" ><?php echo  $tax_term->name ?></a>
                        </div>
                    <?php
                    }
                    ?>
                </div>
            </div>
            <div class="home-duan">
                <h3 class="title-home title-home-dichvu">Dự án đã thực hiện</h3>
                <ul id="home-duan">
                    <?php
                    $featured = new WP_Query('posts_per_page=4&cat=5&orderby=rand');
                    while($featured->have_posts()) : $featured->the_post();
                        $image_attributes = wp_get_attachment_image_src(get_post_thumbnail_id($post->ID), 'full');
                        $params = array('width' => 230, 'height' => 150);

                        $img_category = bfi_thumb($image_attributes[0], $params);
                        ?>
                        <li class="duan-item">
                            <a href="<?php the_permalink(); ?>">
                                <img src="<?php echo $img_category ?>" alt=""/>
                            </a>
                            <div class="duan-item-content">
                                <h3><a href="<?php the_permalink(); ?>"><?php the_title() ?></a></h3>
                                <p><?php echo substr(get_the_content(), 0, 300); ?>...</p>
                            </div>
                        </li>
                    <?php
                    endwhile;
                    wp_reset_postdata();
                    ?>
                </ul>
            </div>
            <div class="doitac-home">
                <h3 class="title-home title-home-doitac">Đối tác</h3>
                <div id="doitac-home" class="owl-carousel owl-theme">
                    <?php
                    if( have_rows('doi_tac','option') ):
                        ?>
                        <?php
                        while ( have_rows('doi_tac','option') ) : the_row();
                            $image_doitac=get_sub_field('image','option');
                            $title_doitac=get_sub_field('name','option');
                            ?>
                            <div class="item">
                                <a href="#"><img src="<?php echo $image_doitac ?>" alt="<?php echo $title_doitac ?>"/></a>
                            </div>
                        <?php
                        endwhile;
                        ?>
                    <?php
                    endif;
                    ?>
                </div>
            </div>
        </div><!-- .row -->
    </div><!-- .container -->
    <div class="contact-home">
        <div class="container">
            <div class="row">
            </div>
        </div>
    </div>
</main><!-- #main -->
<?php get_footer(); ?>
