package com.unosquare.admin_core.back_end.configuration;

import com.unosquare.admin_core.back_end.configuration.mappings.domain.EmployeeMappings;
import com.unosquare.admin_core.back_end.configuration.mappings.presentation.RegisterEmployeeMappings;
import io.github.lukehutch.fastclasspathscanner.FastClasspathScanner;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.config.ConfigurableBeanFactory;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Scope;
import org.springframework.transaction.annotation.EnableTransactionManagement;
import java.lang.*;
import com.unosquare.admin_core.back_end.configuration.mappings.BaseMappings;

import java.util.*;

@Configuration
@EnableTransactionManagement
public class AppConfig {

    @Bean
    @Scope(value = ConfigurableBeanFactory.SCOPE_SINGLETON)
    public ModelMapper modelMapper() {
        ModelMapper modelMapper = new ModelMapper();
        modelMapper.getConfiguration().setAmbiguityIgnored(true);

        List mappings = new ArrayList<>();
        new FastClasspathScanner(EmployeeMappings.class.getPackage().getName())
                .matchSubclassesOf(Object.class, mappings::add)
                .scan();
        new FastClasspathScanner(RegisterEmployeeMappings.class.getPackage().getName())
                .matchSubclassesOf(Object.class, mappings::add)
                .scan();

        for (Object mapping : mappings) {
            if (mapping instanceof BaseMappings<?, ?>) {
                modelMapper.addMappings(((BaseMappings) mapping).MapFromDtoToTarget());
                modelMapper.addMappings(((BaseMappings) mapping).MapFromTargetToDto());
            }

        }
        return modelMapper;
    }
}
