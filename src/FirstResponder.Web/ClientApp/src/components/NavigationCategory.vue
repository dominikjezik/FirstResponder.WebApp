<script>
export default {
    props: {
        isMenuOpenOnLoad: Boolean,
        title: String
    },
    data() {
        return {
            isMenuOpen: this.isMenuOpenOnLoad
        }
    }
};
</script>

<template>
<li class="nav-item">
    <a href="#aed" @click.prevent="isMenuOpen = !isMenuOpen" class="nav-item button is-white">
        <span>
            <slot name="icon"></slot>
            {{ title }}
        </span>

        <svg xmlns='http://www.w3.org/2000/svg' width='16' height='16' viewBox='0 0 16 16'
             class="nav-item-btn-toggle" :class="isMenuOpen ? 'nav-item-btn-toggle-open' : 'nav-item-btn-toggle-close' ">
            <path fill='none' stroke='currentColor' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M5 14l6-6-6-6'/>
        </svg>
    </a>
    <ul class="sub-nav" v-show="isMenuOpen">
        <slot></slot>
    </ul>
</li>
</template>
